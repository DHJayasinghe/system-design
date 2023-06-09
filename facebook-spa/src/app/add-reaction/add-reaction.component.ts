import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add-reaction',
  templateUrl: './add-reaction.component.html',
  styleUrls: ['./add-reaction.component.css']
})
export class AddReactionComponent implements OnInit {
  @Input() postId?:string;
  @Input() commentId?: string;
  @Output() changed = new EventEmitter<boolean>();

  reactionType = ReactionType;

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  react(reactionType: ReactionType) {
    this.http.put(`${environment.baseUrl}/reactions`, { postId: this.postId, commentId: this.commentId, reactionType: reactionType, userId: 1 }).subscribe({
      next: () => { this.changed.emit(true); },
      error: (error) => {
        console.log(error);
      }
    })
  }
}


export enum ReactionType {
  LIKE = 0,
  HEART = 1,
  CARE = 2,
  LAUGH = 3,
  WOW = 4,
  SAD = 5,
  ANGRY = 6
}

