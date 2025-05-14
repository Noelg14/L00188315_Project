import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RegisterDto } from 'src/app/models/LoginDto';
import { AccountService } from 'src/app/services/account-service.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent  {
    constructor(private readonly accountService: AccountService,
      private readonly router: Router,
      private readonly toastr: ToastrService
    ) {}
    loginForm = new FormGroup({
      email: new FormControl('',[Validators.required,Validators.email]),
      password: new FormControl('',Validators.required),
      name: new FormControl('',Validators.required)
    })

  onSubmit(){
      let postData :RegisterDto =  {
          email: this.loginForm.value.email,
          password: this.loginForm.value.password,
          name: this.loginForm.value.name
        };
        this.accountService.register(postData).subscribe({
          next: (response) => {
            console.log(response);
            this.toastr.success('Registered Successful', 'Success');
            this.router.navigateByUrl('/account');
          },
          error: (error) => {
            console.log(error);
            this.toastr.error('Invalid Credentials', 'Error');
          }

      });
    }
}
