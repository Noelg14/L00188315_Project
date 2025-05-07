import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginDto, LoginResponse } from '../models/LoginDto';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    private readonly httpClient: HttpClient,
    private readonly toastr : ToastrService
  ) {
    this.validateToken();
  }

  baseUrl:string = environment.apiUrl;


  login(data: LoginDto) {
    return this.httpClient.post<LoginResponse>(this.baseUrl+'api/Login', data).pipe(
      tap((response: LoginResponse) => { // store the token in the browser local store.
        localStorage.setItem('token', response.token);
      })
    );
  }
  register(data: any){
    return this.httpClient.post<LoginResponse>(this.baseUrl+'api/Register', data).pipe(
      tap((response: LoginResponse) => { // store the token in the browser local store.
        localStorage.setItem('token', response.token);
      })
    );
  }
  validateToken(){
    let token = localStorage.getItem("token") ?? '';
    token?.split('.')
    let data:any = btoa(token[1])
    if(data.exp < Date.now()){
      this.toastr.error("Your token has expired, please login again.")
      return false;
    }
    return true;
  }
}
