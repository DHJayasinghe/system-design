import { Component, OnInit } from '@angular/core';
import { BlobServiceClient } from '@azure/storage-blob';

const blobServiceClient = new BlobServiceClient('SAS_URL');
const containerClient = blobServiceClient.getContainerClient("sample");

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {
  constructor() { }

  ngOnInit() {
  }

  async onFileChange($event: any) {
    console.log('im here');
    const files = $event.target.files;

    if (files && files.length > 0) {
      const file = files[0];
      const blockBlobClient = containerClient.getBlockBlobClient(file.name);
      await blockBlobClient.uploadData(file);
      console.log('File uploaded successfully.');
    }

    // Process other form data and post content here
  }

}
