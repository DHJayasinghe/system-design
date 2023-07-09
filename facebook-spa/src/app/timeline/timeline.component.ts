import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AssetsService } from 'src/services/assets.service';
import { NotificationService } from 'src/services/notification.service';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
})
export class TimelineComponent implements OnInit {
  @Input() version: number = 1;
  posts: Post[] = [];

  constructor(
    private http: HttpClient,
    private assetsService: AssetsService,
    private notificationService: NotificationService) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    this.http.get<Post[]>(`${environment.baseUrl}/posts/timeline`)
      .subscribe(
        (posts) => {
          posts.forEach(post => {
            post.assets = post.assets.map(asset => {
              return this.assetsService.metaInfo?.assetsBaseUrl + "/"
                + this.assetsService.metaInfo?.optimizedImageContainer + "/"
                + this.assetsService.metaInfo?.resolutions[1] + "/"
                + asset.replace("images", "").replace("/", "");
            });
          })
          this.posts = posts;
        }
      );
  }
}

export interface Post {
  id: string;
  content: string;
  authorName: string;
  createdAt: Date;
  updatedAt: Date;
  assets: string[];
}