import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account-service.service';
import { LoginDto } from 'src/app/models/LoginDto';
import { ToastrService } from 'ngx-toastr';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  constructor(private readonly accountService: AccountService,
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly toastr: ToastrService,
    private readonly title: Title
  ) {
    this.title.setTitle("Login")
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];
  }

  loginForm = new FormGroup({
    email: new FormControl('',[Validators.required,Validators.email]),
    password: new FormControl('',Validators.required)
  })

  returnUrl: string;

  onSubmit(){
    let postData :LoginDto =  {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    };
    this.accountService.login(postData).subscribe({
      next: (response) => {
        console.log(response);
        this.toastr.success('Login Successful', 'Success');
        this.router.navigateByUrl(this.returnUrl??'/account');
      },
      error: (error) => {
        console.error(error);
        this.toastr.error('Invalid Credentials', 'Error');
      }

  });
}

}
