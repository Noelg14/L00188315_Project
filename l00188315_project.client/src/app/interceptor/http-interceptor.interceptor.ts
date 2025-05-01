import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class HttpInterceptorInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let token = this.getTokenFromLocalStorage();
    if (token) {
    request = request.clone({ //clone request and set token
          setHeaders: { Authorization: 'Bearer '+ token}
    })
  }
    return next.handle(request);
  }
  private getTokenFromLocalStorage(){
    return localStorage.getItem('token');
  }
}
