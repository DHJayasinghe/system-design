import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-people-you-may-know',
  templateUrl: './people-you-may-know.component.html',
  styleUrls: ['./people-you-may-know.component.css']
})
export class PeopleYouMayKnowComponent implements OnInit {
  friendSuggestions: User[] = [];
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<User[]>(`${environment.baseUrl}/users`).subscribe(result => {
      this.friendSuggestions = result;
    });
  }

  addFriend(d: User) {
    this.http.post(`${environment.baseUrl}/friends`, {
      friendId: d.id
    }).subscribe({
      next: () => { },
      error: (error) => {

      }
    })

  }
}

export interface User {
  id: string,
  email: string,
  phoneNumber: string,
  firstName: string,
  surname: string,
  profilePic: string,
  mutualFriends: string[]
}