import ***REMOVED*** HttpClient ***REMOVED*** from '@angular/common/http';
import ***REMOVED*** Component, Input, OnInit ***REMOVED*** from '@angular/core';
import ***REMOVED*** environment ***REMOVED*** from 'src/environments/environment';

@Component(***REMOVED***
  selector: '***REMOVED***-add-reaction',
  templateUrl: './add-reaction.component.html',
  styleUrls: ['./add-reaction.component.css']
***REMOVED***)
export class AddReactionComponent implements OnInit ***REMOVED***
  @Input() postId = "";
  @Input() commentId = "";
  reactionType = ReactionType;

  constructor(private http: HttpClient) ***REMOVED*** ***REMOVED***

  ngOnInit() ***REMOVED***
  ***REMOVED***

  react(reactionType: ReactionType) ***REMOVED***
    this.http.put(`$***REMOVED***environment.baseUrl***REMOVED***/reactions`, ***REMOVED*** postId: this.postId, commentId: this.commentId, reactionType: reactionType, userId: 1 ***REMOVED***).subscribe(***REMOVED***
      next: () => ***REMOVED*** ***REMOVED***,
      error: (error) => ***REMOVED***
        console.log(error);
  ***REMOVED***
***REMOVED***)
  ***REMOVED***
***REMOVED***


export enum ReactionType ***REMOVED***
  LIKE = 0,
  HEART = 1,
  CARE = 2,
  LAUGH = 3,
  WOW = 4,
  SAD = 5,
  ANGRY = 6
***REMOVED***

