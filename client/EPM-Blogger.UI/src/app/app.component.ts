import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import { HomeComponent } from './General/home/home.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [RouterOutlet, CommonModule, HomeComponent]
})
export class AppComponent implements OnInit {
  showHome = true;

  constructor(private router: Router) {}
  ngOnInit(): void {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        const hiddenRoutes = ['/login', '/register'];
        this.showHome = !hiddenRoutes.some(route => event.urlAfterRedirects.startsWith(route));
      });
  }

  // Get username from localStorage
  getUserName(): string | null {
    const user = localStorage.getItem("user");
    return user ? JSON.parse(user).username : "Guest User";
  }
}
