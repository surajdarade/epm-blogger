import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LikeRequestDto {
  userId: number;
  postId: number;
}

export interface UnLikeRequestDto {
  userId: number;
  postId: number;
}

export interface LikeResponseDto {
  postId: number;
  likeCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  private apiUrl = environment.apiUrl + '/likes';  

  constructor(private http: HttpClient) {}

  // Method to like a post
  likePost(likeRequest: LikeRequestDto): Observable<LikeResponseDto> {
    return this.http.post<LikeResponseDto>(`${this.apiUrl}/add`, likeRequest);
  }
  // Method to get like count for a specific post
  getLikeCount(postId: number): Observable<{ likes: number }> {
    return this.http.get<{ likes: number }>(`${this.apiUrl}/count/${postId}`);
  }
  // Method to dislike (remove like) from a post
  disLikePost(unLikeRequest: UnLikeRequestDto): Observable<LikeResponseDto> {
    return this.http.post<LikeResponseDto>(`${this.apiUrl}/remove`, unLikeRequest);
  }

   checkUserLikedPost(userId: number, postId: number): Observable<{ liked: boolean }> {
    return this.http.get<{ liked: boolean }>(`${this.apiUrl}/check`, {
      params: {
        userId: userId.toString(),
        postId: postId.toString()
      }
    });
  }
}
