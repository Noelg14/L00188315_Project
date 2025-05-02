import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login/login.component';
import { RegisterComponent } from './register/register/register.component';
import { AccountComponent } from './account/account.component';
import { AccountDetailComponent } from './account/account-detail/account-detail.component';

const routes: Routes = [
  {path:'',component: HomeComponent},
  {path:'login',component:LoginComponent},
  {path:'register',component:RegisterComponent},
  {path:'account',component:AccountComponent,pathMatch:'full'  },
  {path:'account/:id',component:AccountDetailComponent},

  // {
  //   path:'checkout',
  //   canActivate:[AuthGuard],
  //   loadChildren:()=>import('./checkout/checkout.module').then(m=>m.CheckoutModule)
  // }, //now lazy loaded

  {path:'**',redirectTo:'',pathMatch:'full'}, //route that does not exist

];

@NgModule({
  imports: [
    RouterModule.forRoot(
      routes,
      { enableTracing: true }
    )
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
