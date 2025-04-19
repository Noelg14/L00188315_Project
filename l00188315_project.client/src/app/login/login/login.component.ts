import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account-service.service';
import { LoginDto } from 'src/app/models/LoginDto';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private accountService: AccountService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {

    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/';
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
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (error) => {
        console.error(error);
        alert("Invalid Credentials");
      }

  });
}

}
