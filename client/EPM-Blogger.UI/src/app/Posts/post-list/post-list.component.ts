import { Component, OnInit } from '@angular/core';
import { PostService } from '../../Services/post-service.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { UserService } from '../../Services/user.service';
import { LikeService } from '../../Services/like.service';
import { environment } from '../../../environments/environment';
import { DetailComponent } from '../detail/detail.component';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.css'],
  imports: [CommonModule,DetailComponent, ReactiveFormsModule, HttpClientModule, FormsModule, RouterModule]
})
export class PostListComponent implements OnInit {
  posts: any[] = [];
  likedPosts = new Set<number>();
  userMap: { [key: number]: string } = {};
  likeCounts: { [postId: number]: number } = {};

  constructor(
    private readonly postService: PostService,
    private readonly router: Router,
    private readonly userService: UserService,
    private readonly likeService: LikeService
  ) { }

  ngOnInit(): void {
    this.postService.getAllPosts().subscribe((data) => {
      this.posts = data;
      const userIds = Array.from(new Set(this.posts.map(post => post.userId)));

      userIds.forEach(userId => {
        this.userService.getUserById(userId).subscribe(user => {
          this.userMap[userId] = user.username;
        });
      });

      this.posts.forEach(post => {
        this.likeService.getLikeCount(post.id).subscribe((response) => {
          this.likeCounts[post.id] = response.likes;
        });
      });

      // User and Checking Liked Posts
      const user = JSON.parse(localStorage.getItem("user") || '{}');
      if (!user || !user.userId) {
        console.warn('User not logged in, skipping liked posts check');
        return;
      }

      // Check if the current user liked each post, update likedPosts set
      this.posts.forEach(post => {
        this.likeService.checkUserLikedPost(user.userId, post.id).subscribe(response => {
          if (response.liked) {
            this.likedPosts.add(post.id);
          }
        });
      });
    });
  }

  // Function to toggle like
  toggleLike(postId: number): void {
    const user = JSON.parse(localStorage.getItem("user") || '{}');
    if (!user || !user.userId) {
      console.error('User is not logged in');
      return;
    }

    const isLiked = this.likedPosts.has(postId);
    if (isLiked) {
      // If post is already liked, call the disLike API
      this.dislikePost(postId, user.userId);
    } else {
      // If post is not liked, call the like API
      this.likePost(postId, user.userId);
    }
  }

  // Handle like action
  private likePost(postId: number, userId: number): void {
    this.likeService.likePost({ userId, postId }).subscribe({
      next: (response) => {
        console.log('Like added:', response);
        this.likedPosts.add(postId);
        this.likeCounts[postId] = response.likeCount;
      },
      error: (error) => {
        console.error('Error liking post:', error);
      }
    });
  }

  // Handle dislike action
  private dislikePost(postId: number, userId: number): void {
    this.likeService.disLikePost({ userId, postId }).subscribe({
      next: (response) => {
        console.log('Like removed:', response);
        this.likedPosts.delete(postId);
        this.likeCounts[postId] = response.likeCount;
      },
      error: (error) => {
        console.error('Error disliking post:', error);
      }
    });
  }

  // Check if a post is liked by the user
  isLiked(postId: number): boolean {
    return this.likedPosts.has(postId);
  }

  sharePost(postId: number): void {
    const url = `${environment.apiUrl}/post/${postId}`;
    navigator.clipboard.writeText(url).then(() => {
      const tooltip = document.getElementById(`tooltip-${postId}`);
      if (tooltip) {
        tooltip.style.display = 'block';
        tooltip.style.opacity = '1';
        setTimeout(() => {
          tooltip.style.opacity = '0';
          setTimeout(() => tooltip.style.display = 'none', 300);
        }, 1500);
      }
    });
  }

  getPreview(content: string, limit: number = 200): string {
    return content.length > limit ? content.substring(0, limit) + '...' : content;
  }

  isLongContent(content: string, limit: number = 200): boolean {
    return content.length > limit;
  }
    // For Detail page
 selectedPostId: number | null = null;

  openPostDetail(postId: number) {
    this.selectedPostId = postId;
  }

   closePostDetail() {
    this.selectedPostId = null;
  }
}
