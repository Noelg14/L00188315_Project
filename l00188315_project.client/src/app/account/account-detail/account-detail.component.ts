import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Account } from 'src/app/models/account';
import { Balance } from 'src/app/models/balance';
import { Transaction } from 'src/app/models/transaction';
import { OpenBankingService } from 'src/app/services/open-banking.service';
import { formatDate } from "@angular/common";

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
    title = "Account | "+(this.account?.currency ?? '');

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
          if(response.data.amount)
            this.balance = response.data;

        },
        error: err =>{ console.error(err)}
      });
      this.obService.transactions(accountId).subscribe({
        next: response =>{
          if(response.data.length > 0)
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
