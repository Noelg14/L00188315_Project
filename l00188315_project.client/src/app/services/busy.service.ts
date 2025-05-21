import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;

  constructor(private readonly spinnerService: NgxSpinnerService) { }

  busy(){
    this.busyRequestCount++;
    this.spinnerService.show(undefined,{
      type:'ball-spin',
      bdColor:'rgba(116, 116, 116, 0.7)',
      color:'#FFF',
    })
  }
  idle(){
    this.busyRequestCount--;
    if(this.busyRequestCount <=0){
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }
}
