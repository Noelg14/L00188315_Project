import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  ngOnInit(): void {
    console.log("[INIT] NavbarComponent initialized");
  }
  toggleTheme(){
    let bodyElement = document.getElementsByTagName("body")
    let currentValue = bodyElement[0].attributes.getNamedItem("data-bs-theme")?.value;
    currentValue === "light" ?
      bodyElement[0].setAttribute("data-bs-theme", "dark") :
      bodyElement[0].setAttribute("data-bs-theme", "light")
  }



}
