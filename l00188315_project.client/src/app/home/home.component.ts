import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account-service.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit{
/**
 *
 */
constructor(
  public readonly accountService:AccountService,
  private readonly title:Title
) {
}
  ngOnInit(): void {
    this.title.setTitle('Home')
  }
}

