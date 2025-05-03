import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginDto, LoginResponse } from '../models/LoginDto';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private readonly httpClient: HttpClient) { }

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
}
