import ***REMOVED*** HttpClient ***REMOVED*** from '@angular/common/http';
import ***REMOVED*** Component, EventEmitter, Input, OnInit, Output ***REMOVED*** from '@angular/core';
import ***REMOVED*** FormBuilder, UntypedFormControl, UntypedFormGroup, Validators ***REMOVED*** from '@angular/forms';
import ***REMOVED*** environment ***REMOVED*** from 'src/environments/environment';
import ***REMOVED*** ReactionCount ***REMOVED*** from '../display-post/display-post.component';

@Component(***REMOVED***
  selector: '***REMOVED***-view-comments',
  templateUrl: './view-comments.component.html',
  styleUrls: ['./view-comments.component.css']
***REMOVED***)
export class ViewCommentsComponent implements OnInit ***REMOVED***
  @Input() postId?: string;
  @Output() dismissed = new EventEmitter<boolean>(false);

  comments: Comment[] = [];
  reactionCounts: ReactionCount[] = [];

  placeCommentForm = new UntypedFormGroup(***REMOVED***
    content: new UntypedFormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(5000)
    ])
  ***REMOVED***);

  constructor(private http: HttpClient) ***REMOVED*** ***REMOVED***

  ngOnInit() ***REMOVED***
    this.getComments();
  ***REMOVED***

  addComment() ***REMOVED***
    if (!this.placeCommentForm.valid) return;
    this.http.post(`$***REMOVED***environment.baseUrl***REMOVED***/posts/$***REMOVED***this.postId***REMOVED***/comments`, this.placeCommentForm.value)
      .subscribe(***REMOVED***
        next: () => ***REMOVED***
          this.placeCommentForm.reset();
          this.getComments();
        ***REMOVED***,
        error: (err) => ***REMOVED***
          console.error(err);
        ***REMOVED***
  ***REMOVED***;
  ***REMOVED***

  private getComments() ***REMOVED***
    this.http.get<Comment[]>(`$***REMOVED***environment.baseUrl***REMOVED***/posts/$***REMOVED***this.postId***REMOVED***/comments`)
      .subscribe(***REMOVED***
        next: (result) => ***REMOVED***
          this.comments = result;
        ***REMOVED***,
        error: (err) => ***REMOVED***
          console.error(err);
        ***REMOVED***
  ***REMOVED***;
  ***REMOVED***

  dismiss() ***REMOVED***
    this.dismissed.emit(true);
  ***REMOVED***

  public getReactionCount() ***REMOVED***
    this.http.get<ReactionCount[]>(`$***REMOVED***environment.baseUrl***REMOVED***/reactions?postId=$***REMOVED***this.postId***REMOVED***`).subscribe(result => ***REMOVED***
      this.reactionCounts = result;
      this.mapReactionCount();
***REMOVED***;
  ***REMOVED***

  private mapReactionCount() ***REMOVED***
    this.reactionCounts.filter(reaction => reaction.commentId != null).forEach(reaction => ***REMOVED***
      var comment = this.comments.filter(comment => comment.id == reaction.commentId)[0];
      comment.totalReactions = reaction.totalReactions;
***REMOVED***;
  ***REMOVED***


***REMOVED***

export interface Comment ***REMOVED***
  id: string;
  postId: string;
  authorName: string;
  content: string;
  createdAt: string;
  showReactionButtons: boolean;
  totalReactions: number;
***REMOVED***
