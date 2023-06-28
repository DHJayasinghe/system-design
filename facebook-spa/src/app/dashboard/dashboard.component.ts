import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  timelineVersion = 1;

  constructor() { }

  ngOnInit() {
  }

  public refreshTimeline(){
    this.timelineVersion = new Date().getTime();
  }
}
