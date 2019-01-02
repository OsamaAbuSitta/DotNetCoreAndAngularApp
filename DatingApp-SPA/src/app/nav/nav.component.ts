import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AlertifyService } from '../../services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
 
  user: any = {};
  constructor(public authService: AuthService,
    private alertifyService:AlertifyService,
    private router:Router) { }

  ngOnInit() {
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
    localStorage.removeItem('token');
    this.router.navigate(['/home']);
  }

}
