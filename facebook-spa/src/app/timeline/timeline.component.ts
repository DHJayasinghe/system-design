import ***REMOVED*** HttpClient ***REMOVED*** from '@angular/common/http';
import ***REMOVED*** Component, Input, OnInit, SimpleChanges ***REMOVED*** from '@angular/core';
import ***REMOVED*** environment ***REMOVED*** from 'src/environments/environment';

@Component(***REMOVED***
  selector: '***REMOVED***-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
***REMOVED***)
export class TimelineComponent implements OnInit ***REMOVED***
  @Input() version: number = 1;
  posts: Post[] = [];

  constructor(private http: HttpClient) ***REMOVED*** ***REMOVED***

  ngOnInit() ***REMOVED***
  ***REMOVED***

  ngOnChanges(changes: SimpleChanges) ***REMOVED***
    this.http.get<Post[]>(`$***REMOVED***environment.baseUrl***REMOVED***/posts`)
      .subscribe(
        (posts) => ***REMOVED***
          this.posts = posts;
        ***REMOVED***
      );
  ***REMOVED*** 
***REMOVED***

export interface Post ***REMOVED***
  id: string;
  content: string;
  authorName: string;
  createdAt: Date;
  updatedAt: Date;
  assets: string[];
***REMOVED***