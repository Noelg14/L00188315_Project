import { Component, OnInit } from '@angular/core';
import { OpenBankingService } from '../services/open-banking.service';
import { Account } from '../models/account';
import { Balance } from '../models/balance';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit{
  constructor(private readonly openBankingService:OpenBankingService) {
  }
  public accounts : Account[] | null = null;
  public balances : Balance[] | null = null;
  ngOnInit(): void {
  this.openBankingService.accounts().subscribe({
      next: (response) => {
        this.accounts = response.data;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }
  linkAccount():void{
    this.openBankingService.consent().subscribe({
      next: (response) => {
        console.log(response);
        window.location.href = response.data;
      },
      error: (error) => {
        console.error(error);
      }
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
