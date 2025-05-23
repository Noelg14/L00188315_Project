import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Account } from 'src/app/models/account';
import { Balance } from 'src/app/models/balance';
import { Transaction } from 'src/app/models/transaction';
import { OpenBankingService } from 'src/app/services/open-banking.service';


@Component({
  selector: 'app-account-detail',
  templateUrl: './account-detail.component.html',
  styleUrls: ['./account-detail.component.scss']
})
export class AccountDetailComponent implements OnInit {
  account?:Account;
  balance?:Balance;
  transactions?: Transaction[] | null;

  constructor(
    private readonly obService : OpenBankingService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly toastr : ToastrService,
  private readonly router: Router) {}
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
        error: err =>{
          this.toastr.error(err.error.message,err.status)
          console.error(err)
        }
      });
      this.obService.transactions(accountId).subscribe({
        next: response =>{
          if(response.data.length > 0)
            this.transactions = response.data;

          console.log(this.transactions)
        },
        error: err =>{
          this.toastr.error(err.error.message,err.status)
          console.error(err)}

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
    deleteAccount(){
      const accountId = this.activatedRoute.snapshot.paramMap.get('id') as string;
      this.obService.deleteAccount(accountId).subscribe({
        next: response =>{
          this.toastr.success("Account Removed", "Deleted")
          document.getElementById("modalClose")?.click(); // force close it
          this.router.navigateByUrl('/account')
        },
        error: err =>{
          this.toastr.error(err.error.message, err.status)
          console.error(err)
        }
      })
    }

}
