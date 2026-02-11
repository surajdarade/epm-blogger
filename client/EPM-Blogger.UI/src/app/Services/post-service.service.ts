import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PostService {

  private baseUrl = 'https://localhost:7002/api/Posts';

  constructor(private http: HttpClient) { }

  getPostById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${id}`);
  }

  getAllPosts(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl);
  }
  getAllPostsByUserId(userId: number): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + "/user/" + userId);
  }
  createPost(post: Post): Observable<Post> {
    return this.http.post<Post>(this.baseUrl, post);
  }

}

export interface Post {
  title: string;
  content: string;
  userId: number;
}
