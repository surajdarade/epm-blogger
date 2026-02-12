import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { Router } from '@angular/router';
import { PostService } from '../../Services/post-service.service';
import { Post } from '../../Services/post-service.service';
@Component({
  selector: 'app-create-post',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule]
})
export class CreateComponent {

  postForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private readonly postService: PostService,
    private readonly router: Router
  ) {
    this.postForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      content: ['', [Validators.required]],
    });
  }

  submitPost(): void {
    if (this.postForm.invalid) return;

    const user = JSON.parse(localStorage.getItem('user') || '{}');
    if (!user || !user.userId) {
      alert('User not logged in!');
      return;
    }

    const newPost: Post = {
      ...this.postForm.value,
      userId: user.userId
    };

    this.postService.createPost(newPost).subscribe({
      next: (res) => {
        alert('Post created successfully!');
        this.router.navigate(['/posts']); // redirect to feed
      },
      error: (err) => {
        console.error(err);
        alert('Failed to create post!');
      }
    });
  }
}
