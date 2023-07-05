import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BlobServiceClient, BlockBlobClient, BlockBlobStageBlockOptions } from '@azure/storage-blob';
import { environment } from 'src/environments/environment';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {
  @Output() posted = new EventEmitter<boolean>();
  private sasToken: string = "";
  private container: string = "";
  private assetsToUpload: string[] = [];


  content: string = "";
  progress: number = 0;
  saving: boolean = false;


  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getUploadLink();
  }

  async onFileChange($event: any) {
    const files = $event.target.files as File[];

    const blobServiceClient = new BlobServiceClient(this.sasToken);
    const containerClient = blobServiceClient.getContainerClient(this.container);

    if (files && files.length > 0) {
      const file = files[0];
      const fileName = `${uuidv4()}.${file.name.split('.').pop()}`;
      const blockBlobClient = containerClient.getBlockBlobClient(fileName);
      await this.uploadAsChunksAsync(file, blockBlobClient);
      this.assetsToUpload.push(fileName);
    }
  }

  private async uploadAsChunksAsync(file: File, blockBlobClient: BlockBlobClient) {
    let uploadedBytes = 0;
    const blockIDs: string[] = [];
    const chunkSize = 1024 * 1024; // 1MB chunk size (adjust as needed)
    const uploadOptions: BlockBlobStageBlockOptions = {
      onProgress: () => {
        const progress = Math.round((uploadedBytes / file.size) * 100);
        this.progress = progress > 100 ? 100 : progress;
      }
    };

    let offset = 0;
    let blockNum = 0;

    while (offset < file.size) {
      uploadedBytes += chunkSize;
      const chunk = file.slice(offset, offset + chunkSize);
      const blockId = btoa(`block-${blockNum}`);
      await blockBlobClient.stageBlock(
        blockId,
        chunk,
        chunk.size,
        uploadOptions
      );
      blockIDs.push(blockId);

      offset += chunkSize;
      blockNum++;
    }
    await blockBlobClient.commitBlockList(blockIDs);
  }

  private getUploadLink(): void {
    this.http.get<any>(`${environment.baseUrl}/assets/upload-link`)
      .subscribe(
        (response: { container: string, sasToken: string }) => {
          const { container, sasToken } = response;
          this.sasToken = sasToken;
          this.container = container;
        }
      );
  }

  public savePost() {
    if (this.saving) return;

    this.saving = true;

    const body = {
      content: this.content,
      assets: this.assetsToUpload
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    this.http.post<any>(`${environment.baseUrl}/posts`, body, { headers })
      .subscribe(
        {
          next: response => {
            console.log(response);
            this.newPost();
          },
          error: error => {
            console.error(error);
          }
        }
      );
  }

  public newPost() {
    this.posted.emit(true);
    this.saving = false;
    this.assetsToUpload = [];
    this.content = "";
    this.progress = 0;
  }

  public disabled(): boolean {
    return this.assetsToUpload.length === 0 || this.saving;
  }
}