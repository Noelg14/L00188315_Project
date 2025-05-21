import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { finalize, identity, Observable } from 'rxjs';
import { BusyService } from '../services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private readonly busyService: BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if
    (
      request.url.includes('emailExists') || request.url.includes('me')
    ){
      return next.handle(request);
    }
    this.busyService.busy();
    return next.handle(request).pipe(
      identity,
      finalize(() => this.busyService.idle())
    )
  }
}
