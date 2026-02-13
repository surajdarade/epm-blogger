import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { environment } from '../../../environments/environment.prod';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, HttpClientModule, RouterModule]
})
export class RegisterComponent {
  registerData = {
    username: '',
    email: '',
    password: ''
  };

  private baseUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient, private router: Router) {}

  onSubmit() {
    this.http.post(`${this.baseUrl}/register`, this.registerData)
      .subscribe({
        next: (res) => {
          const choice = confirm("User registered successfully!\n\nClick OK to go to Login page, or Cancel to stay here.");

          if (choice) {
            this.router.navigate(['/login']);
          } else {
            this.registerData = { username: '', email: '', password: '' };
          }
        },
        error: (err) => {
          alert('Registration failed: ' + err.error.message);
        }
      });
  }
}
