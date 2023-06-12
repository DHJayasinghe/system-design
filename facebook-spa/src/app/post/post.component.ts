import ***REMOVED*** HttpClient, HttpHeaders ***REMOVED*** from '@angular/common/http';
import ***REMOVED*** Component, OnInit ***REMOVED*** from '@angular/core';
import ***REMOVED*** BlobServiceClient, BlockBlobClient, BlockBlobStageBlockOptions ***REMOVED*** from '@azure/storage-blob';
import ***REMOVED*** v4 as uuidv4 ***REMOVED*** from 'uuid';

@Component(***REMOVED***
  selector: '***REMOVED***-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
***REMOVED***)
export class PostComponent implements OnInit ***REMOVED***
  private sasToken: string = "";
  private container: string = "";
  private assetsToUpload: string[] = [];
  public progress: number = 0;

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
      console.log('File uploaded successfully.');
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
    const baseUrl: string = "http://localhost:8083";
    this.http.get<any>(`$***REMOVED***baseUrl***REMOVED***/assets/upload-link`)
      .subscribe(
        (response: ***REMOVED*** container: string, sasToken: string ***REMOVED***) => ***REMOVED***
          const ***REMOVED*** container, sasToken ***REMOVED*** = response;
          this.sasToken = sasToken;
          this.container = container;
    ***REMOVED***
      );
  ***REMOVED***

  public savePost() ***REMOVED***
    const baseUrl: string = "http://localhost:8084";
    const body = ***REMOVED***
      description: 'Sample description',
      assets: this.assetsToUpload
***REMOVED***;
    const headers = new HttpHeaders(***REMOVED***
      'Content-Type': '***REMOVED***lication/json'
***REMOVED***);

    this.http.post<any>(`$***REMOVED***baseUrl***REMOVED***/posts`, body, ***REMOVED*** headers ***REMOVED***)
      .subscribe(
        (response) => ***REMOVED***
          this.assetsToUpload = [];
    ***REMOVED***
      );
  ***REMOVED***

  public disabled(): boolean ***REMOVED***
    return this.assetsToUpload.length === 0;
  ***REMOVED***
***REMOVED***
