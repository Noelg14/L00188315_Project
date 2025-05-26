import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import * as LogRocket from 'logrocket';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(private readonly title:Title) {
    title.setTitle('OpenBanking Dashboard');
    LogRocket.init('xolkcf/project');
  }

}
