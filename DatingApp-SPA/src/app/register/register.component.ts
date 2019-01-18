import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../../app/_services/auth.service';
import { AlertifyService } from '../../app/_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() onCancel = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService, private alertifyService: AlertifyService,
    private fb: FormBuilder,private router:Router) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    }
    this.createRegisterForm();
  }

  createRegisterForm() {
    // this.registerForm = new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   password: new FormControl('',
    //     [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', [Validators.required]),
    // }, this.passwordMatchValidator );

    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required],
    }, {
        Validators: this.passwordMatchValidator
      });
  }

  passwordMatchValidator(group: FormGroup) {
    return group.get("password").value === group.get("confirmPassword").value ? null : { 'mismatch': true };
  }

  onCancelClick() {
    this.onCancel.emit();
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);

      this.authService.register(this.user).subscribe(res => {
        this.alertifyService.success("Registration successful :)");
      },error => {
        this.alertifyService.error(error);
      },()=> {
        this.authService.login(this.user).subscribe(()=>{
          this.router.navigate(['/members'])
        } );
      });
    }

    // this.authService.register(this.user).subscribe( res=>{
    //   this.alertifyService.success("Registration successful :)");
    //   this.onCancelClick();
    // } );
    console.log(this.registerForm.value);
  }
}
