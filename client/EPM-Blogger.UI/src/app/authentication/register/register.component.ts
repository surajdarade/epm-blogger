import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';

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

  constructor(private http: HttpClient,private router:Router) {}

  onSubmit() {
    this.http.post('https://localhost:7002/api/auth/register', this.registerData)
      .subscribe({
        next: (res) => {
          const choice = confirm("User registered successfully!\n\nClick OK to go to Login page, or Cancel to stay here.");

          if (choice) {
            this.router.navigate(['/login']);  
          } else {
            this.registerData = { username: '', email: '', password: '' };
          }
          console.log(res);
        },
        error: (err) => {
          alert('Registration failed: ' + err.error.message);
          console.error(err);
        }
      });
  }
}
