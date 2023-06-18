import ***REMOVED*** Component, Input, OnInit ***REMOVED*** from '@angular/core';
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

  constructor() ***REMOVED*** ***REMOVED***

  ngOnInit() ***REMOVED***
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
***REMOVED***
