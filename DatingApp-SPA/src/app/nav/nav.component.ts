import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../app/_services/auth.service';
import { AlertifyService } from '../../app/_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
 
  user: any = {};
  photoUrl: string;
  constructor(public authService: AuthService,
    private alertifyService:AlertifyService,
    private router:Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
     this.authService.login(this.user).subscribe(res => {
        this.alertifyService.success("Welcome ..."); 
      }, error=> {
        this.alertifyService.error(error);
      }, ()=> { 
        this.router.navigate(['/members']);
      });
  }

  logout() {
    this.authService.logout();
  }

}
