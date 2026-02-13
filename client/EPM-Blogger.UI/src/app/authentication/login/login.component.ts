import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { environment } from '../../../environments/environment.prod';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, HttpClientModule, RouterModule]
})
export class LoginComponent {
  loginData = {
    UsernameOrEmail: '',
    password: ''
  };

  loading = false;
  errorMessage = '';

  private baseUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient, private router: Router) {}

  onSubmit() {
    this.loading = true;
    this.errorMessage = '';

    this.http.post<any>(`${this.baseUrl}/login`, this.loginData)
      .subscribe({
        next: (res) => {
          this.loading = false;

          localStorage.setItem('accessToken', res.accessToken);
          localStorage.setItem('refreshToken', res.refreshToken);
          localStorage.setItem('user', JSON.stringify(res.user));
          localStorage.setItem('isLoggedIn', "true");

          alert("Login successful!");
          this.router.navigate(['/']);
        },
        error: () => {
          this.loading = false;
          this.errorMessage = 'Login failed. Please check your credentials and try again.';
        }
      });
  }
}
