import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.apiUrl + '/users';

  constructor(public readonly http :HttpClient) { }

  getUserById(userId:number): Observable<User>{
    return this.http.get<User>(`${this.apiUrl}/${userId}`);
  }
  
  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

}
export interface User{
  userId:number;
  username : string;
  email : string;
  createdAt : Date;
}
