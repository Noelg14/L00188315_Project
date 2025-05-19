import { Component, OnInit } from '@angular/core';
import { OpenBankingService } from '../services/open-banking.service';
import { ToastrService } from 'ngx-toastr';
import { Transaction } from '../models/transaction';

@Component({
  selector: 'app-transaction-detail',
  templateUrl: './transaction-detail.component.html',
  styleUrls: ['./transaction-detail.component.scss']
})
export class TransactionDetailComponent implements OnInit {
  constructor(
    private readonly openBankingService : OpenBankingService,
    private readonly toastr : ToastrService
  ) {}
  transactions : Transaction[] = [];
  ngOnInit(): void {
    this.openBankingService.allTransactions().subscribe({
      next: (response) => {
        this.transactions = response.data;
      },
      error: (error) => {
        this.toastr.error(error.error.message);
      }
    })
  }

}
