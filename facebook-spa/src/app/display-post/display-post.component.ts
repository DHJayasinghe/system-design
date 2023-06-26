import ***REMOVED*** HttpClient ***REMOVED*** from '@angular/common/http';
import ***REMOVED*** Component, Input, OnInit ***REMOVED*** from '@angular/core';
import ***REMOVED*** environment ***REMOVED*** from 'src/environments/environment';
import ***REMOVED*** Post ***REMOVED*** from '../timeline/timeline.component';

@Component(***REMOVED***
  selector: '***REMOVED***-display-post',
  templateUrl: './display-post.component.html',
  styleUrls: ['./display-post.component.css']
***REMOVED***)
export class DisplayPostComponent implements OnInit ***REMOVED***
  @Input() post?: Post;
  showCommentSection = false;
  showReactionButtons = false;
  reactionCount?: ReactionCount;

  constructor(private http: HttpClient) ***REMOVED*** ***REMOVED***

  ngOnInit() ***REMOVED***
    this.getReactionCount();
  ***REMOVED***

  getPostedTime(date: Date) ***REMOVED***
    const now = new Date();
    const postedDate = new Date(date);

    const timeDiff = now.getTime() - postedDate.getTime();
    const seconds = Math.floor(timeDiff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (seconds < 60) ***REMOVED***
      return 'Just now';
    ***REMOVED*** else if (minutes < 60) ***REMOVED***
      return `$***REMOVED***minutes***REMOVED*** minutes ago`;
    ***REMOVED*** else if (hours < 24) ***REMOVED***
      return `$***REMOVED***hours***REMOVED*** hours ago`;
    ***REMOVED*** else if (days === 1) ***REMOVED***
      return 'Yesterday';
    ***REMOVED*** else ***REMOVED***
      return `$***REMOVED***days***REMOVED*** days ago`;
    ***REMOVED***
  ***REMOVED***

  public getReactionCount() ***REMOVED***
    this.http.get<ReactionCount[]>(`$***REMOVED***environment.baseUrl***REMOVED***/reactions?postId=$***REMOVED***this.post?.id***REMOVED***`).subscribe(result => ***REMOVED***
      console.log(result);
      this.reactionCount = result.filter(d => d.postId == this.post?.id && d.commentId == null)[0];
      console.log(this.reactionCount);
***REMOVED***;
  ***REMOVED***
***REMOVED***

export interface ReactionCount ***REMOVED***
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
***REMOVED***