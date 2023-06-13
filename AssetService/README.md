# Asset Service
Purpose of this service is to handle assets (images, videos, etc) uploaded by users for Posts & Comments.

## Functional Requirements

1. Upload Attachments - Users should be able to upload any type of attachment (images, videos, etc)
2. Remove Attachments - Users should be able to remove any uploaded attachment

## Non-Functional Requirements

1. Low Latency - Users might me accessing the assets related to a post from different part of the globe. So, downloading these assets to users devices should have low latency.
2. Fast Rendering - Users might me accessing the application using various devices, such as mobile phones, web browser, etc. So rendering the original image on these devices can be very slow if the original image size is very large (Ex: 2MB, 5MB sizes). So, we need to optimize the images by creating resized versions of images of the original and serve it based on the device resolution.
3. High Availability - Users might be uploading very large assets files, such as videos & HD images. We don't need this traffic hitting our backend services. We should consider offloading this traffic another such as storage accounts. Therefore our backend services need to be highly available.
4. Security - Only authorized application should be allowed to save assets.
5. Storage & Cost - There can be assets idle without accessing for very long time. For an example, when a Post loose it's interaction (popularity) after it's initial creation, the images or the videos related to that post won't be accessing by any of the viewers. If that assets is not accessing frequently, that would cost us for the storage. So we should consider moving these items to cheaper storage.

![AssetService](https://github.com/DHJayasinghe/system-design/assets/26274468/f4170155-2791-407a-891c-923c31886c89)


## Implementation Considerations
We gonna use Azure Storage Account Blob Storage as the storage to store users Assets (images, videos, etc), cause the Storage Account service is Highly Available, Fault Tolerant and also provide disaster recovery solutions such as Data Redundancy across different Zones and Regions. Also, the BlockBlob blob typs provides by Blob Storage is highly optimized for dealing with large files and also streaming. Which is great for dealing with uploading Videos.
We gonna use 2 Storage Accounts one called **Assets** - for storing original files. And another one called **AssetsOptimized** - for storing optimized versions of the images.
The Assets Storage Account Blob Storage will have 3 containers.
1. Temp - Temporary storage for files need to be uploaded. Assets will be stayed for 1 day, until it's been copied to another container.
2. Images - Storage container for images
3. Videos - Storage container for videos

The service will container 3 functions with there own resposibiltiy.
1. Upload Function - This HTTP trigger function will responsible for generating SAS token with Write Permission to **Temp** Blob container. The function will generate a SAS token URL which is valid for 10mins, which can be consumed by the frontend to upload the assets as Chunks to Storage Accounts directly, to offload the traffic from the backend service. Which gonna cover **NRF #3**. Having a SAS token valid for small time frame with just write permission to specific container, adhere a good security practice on Least Priviledge Principal, covers our NFR #4. 
2. Save Function - This HTTP trigger function will responsible for copying the assets from **Temp** container to a original container based on the asset type. If the asset is an image, will get copied to **Images** container. If the asset is a video, will get copied to a **Videos** container. This service will be called by a backend service (the post service when the user save the post with links of the saved assets). The copy operation will happen on the Storage Account side, so there is no BLOB download upload operation happens on our backend service.
3. Resize Function - This is a BLOB triggered function which gonna fires when there is an blob uploaded to **Images** container, and do resizing on them to multiple resolutions and saved to **AssetsOptimized** storage account. This will cover our NFR #2.

### Blob Lifecycle Management Policy
Note: To put BLOB lifecycle policy you need to have StorageAccount v2 and **Enable access tracking** option checked on **Lifecycle management** section. 
1. RemoveTemporaryBlobs Policy - This policy will delete all the assets on **Temp** container which have Last access time older than 1 day. This way our Temp container won't get grown with Temporary assets files. 
2. MoveToColdStorage - This policy will move all the assets which have not accessed for 30 days to Cold storage. And back to Hot if it's accessed again. This will address our NFR #5 on storage & cost. So we won't have excessive storage cost for non-frequent acccess data. This will apply for both Storage Accounts according to the diagram.

### Content Delivery Network (CDN)
All the assets on **Assets** & **AssetsOptimized** storage accounts will be delivered to users via Azure CDN profile. Which will gonna cache our content on edge servers in point-of-presence (POP) locations close to the users. That way, users will have minimize latency on downloading content to their devices. Which gonna cover our NFR #1.


