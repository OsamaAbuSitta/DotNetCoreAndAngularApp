import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  currentUser: User;
  decodedToken: any;
  currentPhotoUrl = new BehaviorSubject<string>('../../assets/user.png');

  constructor(private http: HttpClient, private router: Router) { }

  changeMemberPhoto(photoUrl: string) {
    this.currentPhotoUrl.next(photoUrl);
  }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.currentUser = user.user;
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.changeMemberPhoto(user.user.photoUrl);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }

  loggedin(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  refresh(): any {
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token))
      this.decodedToken = this.jwtHelper.decodeToken(token);

    const user = localStorage.getItem('user');
    if (user) {
      this.currentUser = JSON.parse(user);
      this.changeMemberPhoto(this.currentUser.photoUrl);
    }



  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');

    this.router.navigate(['/home']);
  }
}
