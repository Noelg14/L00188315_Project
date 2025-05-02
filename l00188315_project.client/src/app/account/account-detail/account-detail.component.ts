import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { Account } from 'src/app/models/account';
import { Balance } from 'src/app/models/balance';
import { Transaction } from 'src/app/models/transaction';
import { OpenBankingService } from 'src/app/services/open-banking.service';

@Component({
  selector: 'app-account-detail',
  templateUrl: './account-detail.component.html',
  styleUrls: ['./account-detail.component.css']
})
export class AccountDetailComponent implements OnInit {
  account?:Account;
  balance?:Balance;
  transactions?:Transaction[];

  constructor(
    private readonly obService : OpenBankingService,
    private readonly activatedRoute: ActivatedRoute) {}
  ngOnInit(): void {

    this.loadDetails();
  }


    loadDetails(){
      const accountId = this.activatedRoute.snapshot.paramMap.get('id') as string;
      this.obService.accounts().subscribe({
        next: response =>{
          this.account = response.data.filter(x => x.accountId === accountId)[0];
        },
        error: err =>{ console.error(err)}
      });
      this.obService.balances(accountId).subscribe({
        next: response =>{
          this.balance = response.data;
        },
        error: err =>{ console.error(err)}
      });
      this.obService.transactions(accountId).subscribe({
        next: response =>{
          this.transactions = response.data;
        },
        error: err =>{ console.error(err)}
      })
    }
    getCurrencySymbol(currency: string): string {
      switch (currency) {
        case 'GBP':
          return '£';
        case 'EUR':
          return '€';
        case 'USD':
          return '$';
        default:
          return '';
      }
    }
}
