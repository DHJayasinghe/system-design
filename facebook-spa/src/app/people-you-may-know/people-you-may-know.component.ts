import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-people-you-may-know',
  templateUrl: './people-you-may-know.component.html',
  styleUrls: ['./people-you-may-know.component.css']
})
export class PeopleYouMayKnowComponent implements OnInit {
  friendSuggestions= [{ profilePic: '', name: 'Jane', mutualFriends:'John'}]
  constructor() { }

  ngOnInit() {
  }
  addFriend(d:any){}
}
