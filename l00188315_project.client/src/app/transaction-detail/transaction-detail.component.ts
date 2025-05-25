import { Component, OnInit } from '@angular/core';
import { OpenBankingService } from '../services/open-banking.service';
import { ToastrService } from 'ngx-toastr';
import { Transaction } from '../models/transaction';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-transaction-detail',
  templateUrl: './transaction-detail.component.html',
  styleUrls: ['./transaction-detail.component.scss']
})
export class TransactionDetailComponent implements OnInit {
  constructor(
    private readonly openBankingService : OpenBankingService,
    private readonly toastr : ToastrService,
    private readonly title: Title
  ) {}
  transactions : Transaction[] = [];
  ngOnInit(): void {
    this.title.setTitle(`Transactions`);
    this.openBankingService.allTransactions().subscribe({
      next: (response) => {
        this.transactions = response.data;
        this.exportToCSV();
      },
      error: (error) => {
        this.toastr.error(error.error.message);
      }
    })
  }

  exportToCSV(){
    if(this.transactions.length < 1 ){
      this.toastr.error("No transactions to download");
      return;
    }
    const header = "Transaction Date,Account,Currency,Amount,Detail,Type,Status"
    let rows = "";
    for(let transaction of this.transactions){
        let row = `${transaction.bookingDateTime},${transaction.rootAccountId},${transaction.amountCurrency},${transaction.amount},${transaction.transactionInformation	},${transaction.proprietaryBankTransactionCode ?? transaction.creditDebitIndicator},${transaction.status}`+"\n";
        rows += row;
    }
    const csvContent = header + "\n" + rows;
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    link.classList.add('btn');
    link.classList.add('btn-primary');
    link.classList.add('btn-lg');
    link.textContent = 'Download CSV';
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', 'transactions.csv');
    document.getElementById('csvDownload')?.appendChild(link);
  }

}
