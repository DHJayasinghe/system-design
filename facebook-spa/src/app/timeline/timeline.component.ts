import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
})
export class TimelineComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit() {
    const baseUrl: string = "http://localhost:8084";
    this.http.get<any>(`${baseUrl}/posts`)
      .subscribe(
        (response: [{ postId: string, description: string, assets:string[] }]) => {
          console.log(response);
        }
      );
  }

}
