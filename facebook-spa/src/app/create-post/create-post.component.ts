import ***REMOVED*** HttpClient, HttpHeaders ***REMOVED*** from '@angular/common/http';
import ***REMOVED*** Component, EventEmitter, OnInit, Output ***REMOVED*** from '@angular/core';
import ***REMOVED*** BlobServiceClient, BlockBlobClient, BlockBlobStageBlockOptions ***REMOVED*** from '@azure/storage-blob';
import ***REMOVED*** environment ***REMOVED*** from 'src/environments/environment';
import ***REMOVED*** v4 as uuidv4 ***REMOVED*** from 'uuid';

@Component(***REMOVED***
  selector: '***REMOVED***-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
***REMOVED***)
export class CreatePostComponent implements OnInit ***REMOVED***
  @Output() posted = new EventEmitter<boolean>();
  private sasToken: string = "";
  private container: string = "";
  private assetsToUpload: string[] = [];


  content: string = "";
  progress: number = 0;
  saving: boolean = false;


  constructor(private http: HttpClient) ***REMOVED*** ***REMOVED***

  ngOnInit() ***REMOVED***
    this.getUploadLink();
  ***REMOVED***

  async onFileChange($event: any) ***REMOVED***
    const files = $event.target.files as File[];

    const blobServiceClient = new BlobServiceClient(this.sasToken);
    const containerClient = blobServiceClient.getContainerClient(this.container);

    if (files && files.length > 0) ***REMOVED***
      const file = files[0];
      const fileName = `$***REMOVED***uuidv4()***REMOVED***.$***REMOVED***file.name.split('.').pop()***REMOVED***`;
      const blockBlobClient = containerClient.getBlockBlobClient(fileName);
      await this.uploadAsChunksAsync(file, blockBlobClient);
      this.assetsToUpload.push(fileName);
    ***REMOVED***
  ***REMOVED***

  private async uploadAsChunksAsync(file: File, blockBlobClient: BlockBlobClient) ***REMOVED***
    let uploadedBytes = 0;
    const blockIDs: string[] = [];
    const chunkSize = 1024 * 1024; // 1MB chunk size (adjust as needed)
    const uploadOptions: BlockBlobStageBlockOptions = ***REMOVED***
      onProgress: () => ***REMOVED***
        const progress = Math.round((uploadedBytes / file.size) * 100);
        this.progress = progress > 100 ? 100 : progress;
      ***REMOVED***
    ***REMOVED***;

    let offset = 0;
    let blockNum = 0;

    while (offset < file.size) ***REMOVED***
      uploadedBytes += chunkSize;
      const chunk = file.slice(offset, offset + chunkSize);
      const blockId = btoa(`block-$***REMOVED***blockNum***REMOVED***`);
      await blockBlobClient.stageBlock(
        blockId,
        chunk,
        chunk.size,
        uploadOptions
      );
      blockIDs.push(blockId);

      offset += chunkSize;
      blockNum++;
    ***REMOVED***
    await blockBlobClient.commitBlockList(blockIDs);
  ***REMOVED***

  private getUploadLink(): void ***REMOVED***
    this.http.get<any>(`$***REMOVED***environment.baseUrl***REMOVED***/assets/upload-link`)
      .subscribe(
        (response: ***REMOVED*** container: string, sasToken: string ***REMOVED***) => ***REMOVED***
          const ***REMOVED*** container, sasToken ***REMOVED*** = response;
          this.sasToken = sasToken;
          this.container = container;
        ***REMOVED***
      );
  ***REMOVED***

  public savePost() ***REMOVED***
    if (this.saving) return;

    this.saving = true;

    const body = ***REMOVED***
      content: this.content,
      assets: this.assetsToUpload
    ***REMOVED***;
    const headers = new HttpHeaders(***REMOVED***
      'Content-Type': '***REMOVED***lication/json'
***REMOVED***;

    this.http.post<any>(`$***REMOVED***environment.baseUrl***REMOVED***/posts`, body, ***REMOVED*** headers ***REMOVED***)
      .subscribe(
    ***REMOVED***
          next: (response) => ***REMOVED***
            console.log(response);
            this.newPost();
          ***REMOVED***,
          error: (error) => ***REMOVED***

          ***REMOVED***
        ***REMOVED***
      );
  ***REMOVED***

  public newPost() ***REMOVED***
    this.posted.emit(true);
    this.saving = false;
    this.assetsToUpload = [];
    this.content = "";
    this.progress = 0;
  ***REMOVED***

  public disabled(): boolean ***REMOVED***
    return this.assetsToUpload.length === 0 || this.saving;
  ***REMOVED***
***REMOVED***