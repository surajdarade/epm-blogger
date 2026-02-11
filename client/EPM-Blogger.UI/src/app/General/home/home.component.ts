import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  getUserName(): string | null {
    const user = localStorage.getItem("user");
    return user ? JSON.parse(user).username : "Guest User";
  }
}
