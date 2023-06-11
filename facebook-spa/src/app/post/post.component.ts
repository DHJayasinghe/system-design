import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BlobServiceClient } from '@azure/storage-blob';
import { v4 as uuidv4 } from 'uuid';



@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {
  private sasToken: string = "";
  private container: string = "";
  private assetsToUpload: string[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getUploadLink();
  }

  async onFileChange($event: any) {
    console.log('im here');
    const files = $event.target.files;

    const blobServiceClient = new BlobServiceClient(this.sasToken);
    const containerClient = blobServiceClient.getContainerClient(this.container);

    if (files && files.length > 0) {
      const file = files[0];
      const fileName = `${uuidv4()}.${file.name.split('.').pop()}`;
      console.log(fileName);
      const blockBlobClient = containerClient.getBlockBlobClient(fileName);
      await blockBlobClient.uploadData(file);
      this.assetsToUpload.push(fileName);
      console.log('File uploaded successfully.');
    }

    // Process other form data and post content here
  }


  private getUploadLink(): void {
    const baseUrl: string = "http://localhost:8083";
    this.http.get<any>(`${baseUrl}/assets/upload-link`)
      .subscribe(
        (response: { container: string, sasToken: string }) => {
          // Handle the response data
          const { container, sasToken } = response;
          console.log('Container:', container);
          console.log('SasToken:', sasToken);
          this.sasToken = sasToken;
          this.container = container;
          // Further process the data as needed
        }
      );
  }

  public savePost() {
    const baseUrl: string = "http://localhost:8084";
    const body = {
      description: 'Sample description',
      assets: this.assetsToUpload
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    this.http.post<any>(`${baseUrl}/posts`, body, { headers })
      .subscribe(
        (response) => {

          console.log('save');
        }
      );
  }
}
