import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { PostService } from '../../Services/post-service.service';
import { UserService } from '../../Services/user.service';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  imports: [DatePipe,CommonModule],
  styleUrls: ['./detail.component.css']
})
export class DetailComponent implements OnInit {
  @Input() postId!: number;
  @Output() close = new EventEmitter<void>();

  post: any;
  author: string = '';

  constructor(private postService: PostService, private userService: UserService) {}

  ngOnInit(): void {
    console.log("Detail Component Post ID:", this.postId);
    if (this.postId) {
      this.postService.getPostById(this.postId).subscribe(post => {
        this.post = post;

        this.userService.getUserById(post.userId).subscribe(user => {
          this.author = user.username;
        });
      });
    }
  }

  onClose() {
    this.close.emit();
  }
}
