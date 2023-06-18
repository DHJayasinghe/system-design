import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { environment } from 'src/environments/environment';

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
    this.http.get<Post[]>(`${environment.baseUrl}/posts`)
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