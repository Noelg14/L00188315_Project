import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountComponent } from './account.component';
import { AppRoutingModule } from '../app-routing.module';
import { AccountDetailComponent } from './account-detail/account-detail.component';
import { BrowserModule, Title } from '@angular/platform-browser';



@NgModule({
  declarations: [
    AccountComponent,
    AccountDetailComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    BrowserModule
  ]
})
export class AccountModule { }
