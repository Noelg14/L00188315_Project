import { Component } from '@angular/core';
import { AccountService } from '../services/account-service.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
/**
 *
 */
constructor(
  public readonly accountService:AccountService
) {
}
}
