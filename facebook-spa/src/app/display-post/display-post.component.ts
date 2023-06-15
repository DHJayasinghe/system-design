import { Component, Input, OnInit } from '@angular/core';
import { Post } from '../timeline/timeline.component';

@Component({
  selector: 'app-display-post',
  templateUrl: './display-post.component.html',
  styleUrls: ['./display-post.component.css']
})
export class DisplayPostComponent implements OnInit {
  @Input() post?: Post;
  constructor() { }

  ngOnInit() {
  }

  getPostedTime(date:Date) {
    const now = new Date();
    const postedDate = new Date(date);

    const timeDiff = now.getTime() - postedDate.getTime();
    const seconds = Math.floor(timeDiff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (seconds < 60) {
      return 'Just now';
    } else if (minutes < 60) {
      return `${minutes} minutes ago`;
    } else if (hours < 24) {
      return `${hours} hours ago`;
    } else if (days === 1) {
      return 'Yesterday';
    } else {
      return `${days} days ago`;
    }
  }

}
