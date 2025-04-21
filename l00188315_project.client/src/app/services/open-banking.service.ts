import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OpenBankingService {

  constructor(private httpClient:HttpClient) { }
  baseUrl:string = environment.apiUrl;
    accounts(){
      return this.httpClient.get(this.baseUrl+'api/Revolut/accounts');
    }
}
