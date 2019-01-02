import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  baseUrl = 'http://localhost:5000/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  constructor(private httpClient: HttpClient) { }

  login(model) {
    return this.httpClient.post(`${this.baseUrl}login`, model).pipe(
      map(res => {
        const user: { token: string } = <{ token: string }>res;
        if (user && user.token) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
        }
      }));
  }

  register(user) {
    return this.httpClient.post(`${this.baseUrl}register`, user);
  }

  loggedin(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  refreshDecodedToken(): any { 
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token))
      this.decodedToken = this.jwtHelper.decodeToken(token);
  }
}
