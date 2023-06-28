import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from '../timeline/timeline.component';

@Component({
  selector: 'app-display-post',
  templateUrl: './display-post.component.html',
  styleUrls: ['./display-post.component.css']
})
export class DisplayPostComponent implements OnInit {
  @Input() post?: Post;
  showCommentSection = false;
  showReactionButtons = false;
  reactionCount?: ReactionCount;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getReactionCount();
  }

  getPostedTime(date: Date) {
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

  public getReactionCount() {
    this.http.get<ReactionCount[]>(`${environment.baseUrl}/reactions?postId=${this.post?.id}`).subscribe(result => {
      console.log(result);
      this.reactionCount = result.filter(d => d.postId == this.post?.id && d.commentId == null)[0];
      console.log(this.reactionCount);
    });
  }
}

export interface ReactionCount {
  id: string,
  postId: string,
  commentId: string,
  likeCount: number,
  heartCount: number,
  wowCount: number,
  careCount: number,
  laughCount: number,
  sadCount: number,
  angryCount: number,
  totalReactions: number
}