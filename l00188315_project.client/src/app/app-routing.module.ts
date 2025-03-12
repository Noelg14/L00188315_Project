import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {path:'',component: HomeComponent},

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
