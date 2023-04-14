# URL Shortener Service

The purpose of this service is to shorten long URL and keep track of the user traffic on how many users click those URLs.

## Functional Requirements

1. When providing a URL should generate a shorter URL
2. When clicking the shorter URL should redirect to the longer URL

## Non-Functional Requirements

1. Low latency
2. High availability
3. Security


## Implementation

![image](https://github.com/DHJayasinghe/system-design/blob/features/url-shorten-service/UrlShortnerService/URL_ShortenerService.png)

To achieve the functional requirement we will have 2 seperate functions.
1. UrlShortenFunction `POST /shorten` This function will take an input of the longer URL and use a simple algorithm to generate a short URL and return it.
2. UrlRedirectFunction `GET /{url}` This function will accept short URL requests come to our applications and find the related long URL, do the redirection.
    
 ## Considerations
 In order to decide the length of the short URL, we should consider below factors;
 1. How much traffic we gonna expect for a second
 2. How long we gonna retain this shorten URL, so we can consider re-using them after it's been expired. 
 3. What characters we could use for the short URL. It's just numerics, alphabets or alpha numerics.

In case let's say;<br>
&emsp;No. of unique short url we should generate by our system -> Y <br>
&emsp;No. of traffic should handle by our system per second -> X <br>
&emsp;No. of years that the generated shorten URL should remain -> Z <br>

Then Y = X * 60 * 60 * 24 * Z;<br>
So if X = 10000 requests, and Z = 5 years<br>
Then Y will be = 10000 * 60 * 24 * 5 => 72 million unique URLs <br>

Considering that, in order to handle 72M unique URLs by our system **we gonna consider using alphanumerics** as the characters, so we could have 62chars (0-9 a-z A-Z) for the pattern. And for the length calculation. If we use;
1. 2 chars -> then it would be 62^2 = 3844 urls (NOT ENOUGH)
2. 4 chars -> then it would be 62^4 = 14M urls (Also NOT ENOUGH)
3. 6 chars -> then it would be 62^6 = 56B urls (ENOUGH)
4. 7 chars -> then it would be 62^7 = 3.5T urls (More than ENOUGH)

Considering these calculations we gonna use, 7chars as the length with alphanumerics (62 chars combination), eventhough it's too much considering our traffic per second. But we gonna consider Facebook, Twitter scale traffic comes per second to our system and design based on that.

## Algorithm
1. Take an Input URL
2. Hash it using SHA256
3. Convert it to BASE64 to give ASCII representation for the HASH
4. Remove non-alphanumerics characters - Base64 encoding will provide 64 ASCII chars, but we planned to use only 62 of them
5. Take only the first 7 characters - This is the length of the URL we planned to use.

## Application
To implement application functionality we have selected Serverless (aka Consumption Plan) Azure Functions HTTP triggers, because we need to be able to scale the application horizontally and maintain our non-functional requirement on **Availability**. In Azure we could use App Services as well to deploy this application as an API. But we don't want to get the burden on creating scaling rules and spending time coding on integration with CosmosDB, which is already pre-built in with Azure Function SDK. So, we are selecting Azure Function App for our Service implementation, because it already handling this scaling and CosmosDB implementation details for me.

Also, this Azure Function app will run on **Consumption Plan** which will give us flexible billing based on the usage. But there could be this **[Cold-Start](https://azure.microsoft.com/en-us/blog/understanding-serverless-cold-start/)** scenario which is unique to serverless model, where Function App infrastructure gonna deallocate after 20min of inactivity of our Service. In that case, warming up time on function app again **will add a latency** to our service which gonna break our non-functional requirment. We could prevent this easily cost effectively by doing a ping to our Function App by an external service every 15min or using a TimerTrigger which run every 15min to keep one instance active and warm up.

## Database
For the database we have selected Azure CosmosDB **NoSQL** database as the persistent storage to store these short URL and related long URL in a **Key-Attribute datastore model**. We could use Relational databases as well, but considering the Facebook, Twitter scale traffic, we need a **geo-distributed database** that could scale horizontally and can handle high read/write throughput. Relational database also could handle high throughput on read/write but it could be slower in long run when the data size is growing and doing querying on them. Which could affect our latency non-functional requirement. Also, having a flexible data model is good for future changing data requirement, specially when comes to applying migrations on changing schema. Considering that we could use cheap NoSQL Azure table storage with Key-Attribute data model as well for the requirment. But it won't give geo-distributed database requriment on the scale we are looking at.

When picking partition key we for the CosmosDB we have to be careful on avoiding **Hot Spot** situation where traffic is distributed to just one physical partition, which can lead to a performance bottleneck, that gonna affect the **availability** of our system. Example is using a date as a partition key, which gonna lead all the traffic to one partition. In our case, we are using ShortUrl as our partition key, so the traffic gonna evently distribute across other partitions.

## Security
We need to protect the `POST /shorten` endpoint cause we should not allow anonymous users to consume this. Only our backend applications should consume this. For that, we gonna use Azure Functions built-in security for the HTTP Trigger. This will provide us an Key to call this particular endpoint when deployed to Azure environment. This will achieve our **Security** non-functional requirement.

![image](https://user-images.githubusercontent.com/26274468/232023018-b60c4e29-7a64-4ca9-a27e-39c2cb299521.png)
    
