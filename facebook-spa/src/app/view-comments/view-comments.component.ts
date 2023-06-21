import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { ReactionCount } from '../display-post/display-post.component';

@Component({
  selector: 'app-view-comments',
  templateUrl: './view-comments.component.html',
  styleUrls: ['./view-comments.component.css']
})
export class ViewCommentsComponent implements OnInit {
  @Input() postId?: string;
  @Output() dismissed = new EventEmitter<boolean>(false);

  comments: Comment[] = [];
  reactionCounts: ReactionCount[] = [];

  placeCommentForm = new UntypedFormGroup({
    content: new UntypedFormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(5000)
    ])
  });

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getComments();
  }

  addComment() {
    if (!this.placeCommentForm.valid) return;
    this.http.post(`${environment.baseUrl}/posts/${this.postId}/comments`, this.placeCommentForm.value)
      .subscribe({
        next: () => {
          this.placeCommentForm.reset();
          this.getComments();
        },
        error: (err) => {
          console.error(err);
        }
      });
  }

  private getComments() {
    this.http.get<Comment[]>(`${environment.baseUrl}/posts/${this.postId}/comments`)
      .subscribe({
        next: (result) => {
          this.comments = result;
        },
        error: (err) => {
          console.error(err);
        }
      });
  }

  dismiss() {
    this.dismissed.emit(true);
  }

  public getReactionCount() {
    this.http.get<ReactionCount[]>(`${environment.baseUrl}/reactions?postId=${this.postId}`).subscribe(result => {
      this.reactionCounts = result;
      this.mapReactionCount();
    });
  }

  private mapReactionCount() {
    this.reactionCounts.filter(reaction => reaction.commentId != null).forEach(reaction => {
      var comment = this.comments.filter(comment => comment.id == reaction.commentId)[0];
      comment.totalReactions = reaction.totalReactions;
    });
  }


}

export interface Comment {
  id: string;
  postId: string;
  authorName: string;
  content: string;
  createdAt: string;
  showReactionButtons: boolean;
  totalReactions: number;
}
