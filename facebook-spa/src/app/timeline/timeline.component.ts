import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
})
export class TimelineComponent implements OnInit {
  @Input() version: number = 1;
  posts: Post[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit() {
  
    
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log(changes['version'].currentValue);
    const baseUrl: string = "http://localhost:8084";
    this.http.get<Post[]>(`${baseUrl}/posts`)
      .subscribe(
        (posts) => {
          this.posts = posts;
        }
      );
  }

}



export interface Post {
  id: number;
  content: string;
  authorName: string;
  createdAt: Date;
  updatedAt: Date;
  assets: string[];
}