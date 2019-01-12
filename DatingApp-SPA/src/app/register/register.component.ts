import { Component, OnInit,Output, EventEmitter } from '@angular/core';
import { AuthService } from '../../app/_services/auth.service';
import { AlertifyService } from '../../app/_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() onCancel = new EventEmitter();
  user:any = {};
  constructor(private authService:AuthService,private alertifyService:AlertifyService) { }

  ngOnInit() {
  }

  onCancelClick(){
    this.onCancel.emit();
  }

  register(){
    this.authService.register(this.user).subscribe( res=>{
      this.alertifyService.success("Registration successful :)");
      this.onCancelClick();
    } );
  }
}
