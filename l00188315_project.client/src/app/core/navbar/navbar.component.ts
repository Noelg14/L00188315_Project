import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  ngOnInit(): void {
    console.log("[INIT] NavbarComponent initialized");
    const storedTheme = localStorage.getItem("theme") ?? "light";// default to light
    this.setTheme(storedTheme)
  }

  toggleTheme(){
    let checked = <HTMLInputElement>document.querySelector("#toggle")
    checked?.checked ?
      this.setTheme("dark") : // true -> enable
      this.setTheme("light")// false -> disable.

  }
  setTheme(theme : string){
    const checked : HTMLInputElement = document.querySelector("#toggle")!;
    const bodyElement = document.getElementsByTagName("body"); // there should only be one body...
    bodyElement[0].setAttribute("data-bs-theme", theme) // true -> enable
    localStorage.setItem("theme", theme);

    if(theme === "dark")
      checked.checked = true;
  }
}
