import ***REMOVED*** Component, OnInit ***REMOVED*** from '@angular/core';

@Component(***REMOVED***
  selector: '***REMOVED***-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
***REMOVED***)
export class DashboardComponent implements OnInit ***REMOVED***
  timelineVersion = 1;

  constructor() ***REMOVED*** ***REMOVED***

  ngOnInit() ***REMOVED***
  ***REMOVED***

  public refreshTimeline()***REMOVED***
    this.timelineVersion = new Date().getTime();
  ***REMOVED***
***REMOVED***
