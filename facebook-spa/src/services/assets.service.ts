import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AssetsService {
  metaInfo?: AssetsMetaInfo;

  constructor(private http: HttpClient) {
    this.http.get<AssetsMetaInfo>(`${environment.baseUrl}/assets/meta-info`).subscribe(result => {
      this.metaInfo = result;
    })
  }

}

export interface AssetsMetaInfo {
  assetsBaseUrl: string;
  optimizedImageContainer: string;
  resolutions: string[];
}