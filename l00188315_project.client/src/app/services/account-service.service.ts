import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, ReplaySubject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginDto, LoginResponse } from '../models/LoginDto';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
    private currentUserSource = new ReplaySubject<LoginResponse | null>(1);
    currentUser$ = this.currentUserSource.asObservable();

  constructor(
    private readonly httpClient: HttpClient,
    private readonly toastr : ToastrService,
    private readonly router: Router
  ) {
    this.validateToken();
  }

  baseUrl:string = environment.apiUrl;

  loadCurrentUser(){
    debugger;
    let token = localStorage.getItem("token")
    if(token === null){
      this.currentUserSource.next(null);
      return of(null);
    }
    let headers = new HttpHeaders();
    headers= headers.set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<LoginResponse>(this.baseUrl+'api/me', {headers: headers}).pipe(
      map(user => {
        if(user){
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
          return user;
        }else{
          return null;
        }

      }
    ));
  }
  login(data: LoginDto) {
    return this.httpClient.post<LoginResponse>(this.baseUrl+'api/Login', data).pipe(
      map((response: LoginResponse) => { // store the token in the browser local store.
        this.currentUserSource.next(response);
        localStorage.setItem('token', response.token);
      })
    );
  }
  register(data: any){
    return this.httpClient.post<LoginResponse>(this.baseUrl+'api/Register', data).pipe(
      map((response: LoginResponse) => { // store the token in the browser local store.
        this.currentUserSource.next(response);
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
      this.router.navigateByUrl('/login');
      return false;
    }
    return true;
  }
  logout(){
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }
}
