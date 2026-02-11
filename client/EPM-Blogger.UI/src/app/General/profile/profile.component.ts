import { Component } from '@angular/core';
import { UserService } from '../../Services/user.service';
import { DatePipe } from '@angular/common';
import { UserPostComponent } from '../../Posts/user-post/user-post.component';

@Component({
  selector: 'app-profile',
  imports: [DatePipe,UserPostComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
user: any = null;
  loading = true;
  error: string | null = null;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    const userId = JSON.parse(localStorage.getItem("user") || '{}').userId;
    console.warn('Fetched userId from localStorage:', userId);
    this.userService.getUserById(userId).subscribe({
      next: (data) => {
        this.user = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to fetch user details', err);
        this.error = 'Unable to load profile';
        this.loading = false;
      }
    });
  }
}
