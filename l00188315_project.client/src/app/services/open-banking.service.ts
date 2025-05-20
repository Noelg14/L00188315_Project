import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Account } from '../models/account';
import { ApiResponse } from '../models/apiResponse';
import { Transaction } from '../models/transaction';
import { Balance } from '../models/balance';

@Injectable({
  providedIn: 'root'
})
export class OpenBankingService {

  constructor(private readonly httpClient:HttpClient) {
}
  baseUrl:string = environment.apiUrl;
    accounts(){
      return this.httpClient.get<ApiResponse<Account[]>>(this.baseUrl+'api/Revolut/accounts');
    }
    transactions(accountId:string){
      return this.httpClient.get<ApiResponse<Transaction[]>>(this.baseUrl+`api/Revolut/transactions?accountId=${accountId}`);
    }
    balances(accountId:string){
      return this.httpClient.get<ApiResponse<Balance>>(this.baseUrl+`api/Revolut/balances?accountId=${accountId}`);
    }
    consent(){
      return this.httpClient.get<ApiResponse<string>>(this.baseUrl+'api/Revolut/consent');
    }
    deleteAccount(accountId : string){
        return this.httpClient.delete<ApiResponse<string>>(this.baseUrl+`api/Revolut/accounts?accountId=${accountId}`);
    }
    allTransactions(){
      return this.httpClient.get<ApiResponse<Transaction[]>>(this.baseUrl+`api/Revolut/transactions/all`);
    }
}
