import { Component, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { PostService } from '../../Services/post-service.service';
import { UserService } from '../../Services/user.service';
import { LikeService } from '../../Services/like.service';
import { environment } from '../../../environments/environment';
import { DetailComponent } from '../detail/detail.component';

@Component({
  selector: 'app-user-posts',
  templateUrl: './user-post.component.html',
  styleUrls: ['./user-post.component.css'],
  standalone: true,
  imports: [CommonModule, DetailComponent, ReactiveFormsModule, HttpClientModule, FormsModule, RouterModule]
})
export class UserPostComponent implements OnInit {
  @Input() userId!: number; 

  posts: any[] = [];
  likedPosts = new Set<number>();
  userMap: { [key: number]: string } = {};
  likeCounts: { [postId: number]: number } = {};
  selectedPostId: number | null = null;

  constructor(
    private readonly postService: PostService,
    private readonly userService: UserService,
    private readonly likeService: LikeService
  ) {}

  ngOnInit(): void {
    if (!this.userId) {
      console.error("❌ userId is required for UserPostsComponent");
      return;
    }
    this.userId = localStorage.getItem("user") ? JSON.parse(localStorage.getItem("user") || '{}').userId : 0;
    console.log("🔹 UserPostsComponent initialized with userId:", this.userId);

    // 🔹 Call getAllPostsByUserId instead of getAllPosts
    this.postService.getAllPostsByUserId(this.userId).subscribe((data) => {
      this.posts = data;

      // Fetch usernames for each post
      const userIds = Array.from(new Set(this.posts.map(post => post.userId)));
      userIds.forEach(userId => {
        this.userService.getUserById(userId).subscribe(user => {
          this.userMap[userId] = user.username;
        });
      });

      // Fetch like counts for each post
      this.posts.forEach(post => {
        this.likeService.getLikeCount(post.id).subscribe((response) => {
          this.likeCounts[post.id] = response.likes;
        });
      });

      // Fetch liked posts for current user
      const user = JSON.parse(localStorage.getItem("user") || '{}');
      if (user && user.userId) {
        this.posts.forEach(post => {
          this.likeService.checkUserLikedPost(user.userId, post.id).subscribe(response => {
            if (response.liked) {
              this.likedPosts.add(post.id);
            }
          });
        });
      }
    });
  }

  toggleLike(postId: number): void {
    const user = JSON.parse(localStorage.getItem("user") || '{}');
    if (!user || !user.userId) return;

    const isLiked = this.likedPosts.has(postId);
    isLiked ? this.dislikePost(postId, user.userId) : this.likePost(postId, user.userId);
  }

  private likePost(postId: number, userId: number): void {
    this.likeService.likePost({ userId, postId }).subscribe((response) => {
      this.likedPosts.add(postId);
      this.likeCounts[postId] = response.likeCount;
    });
  }

  private dislikePost(postId: number, userId: number): void {
    this.likeService.disLikePost({ userId, postId }).subscribe((response) => {
      this.likedPosts.delete(postId);
      this.likeCounts[postId] = response.likeCount;
    });
  }

  isLiked(postId: number): boolean {
    return this.likedPosts.has(postId);
  }

  sharePost(postId: number): void {
    const url = `${environment.apiUrl}/post/${postId}`;
    navigator.clipboard.writeText(url);
  }

  getPreview(content: string, limit: number = 200): string {
    return content.length > limit ? content.substring(0, limit) + '...' : content;
  }

  isLongContent(content: string, limit: number = 200): boolean {
    return content.length > limit;
  }

  openPostDetail(postId: number) {
    this.selectedPostId = postId;
  }

  closePostDetail() {
    this.selectedPostId = null;
  }
}
