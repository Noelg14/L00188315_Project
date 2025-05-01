import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login/login.component';
import { RegisterComponent } from './register/register/register.component';
import { AccountComponent } from './account/account.component';

const routes: Routes = [
  {path:'',component: HomeComponent},
  {path:'login',component:LoginComponent},
  {path:'register',component:RegisterComponent},
  {path:'account',component:AccountComponent},

  // {
  //   path:'checkout',
  //   canActivate:[AuthGuard],
  //   loadChildren:()=>import('./checkout/checkout.module').then(m=>m.CheckoutModule)
  // }, //now lazy loaded

  {path:'**',redirectTo:'',pathMatch:'full'}, //route that does not exist

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
