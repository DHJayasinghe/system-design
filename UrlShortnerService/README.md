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
1. UrlShortenFunction (POST '/shorten'
    This function will take an input of the longer URL and use a simple algorithm to generate short URL. In order to decide the length of the short URL, we should consider below factors;
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

The algorithm will be as follow.
1. Take an Input URL
2. Hash it using SHA256
3. Convert it to BASE64 to give ASCII representation for the HASH
4. Remove non-alphanumerics characters - Base64 encoding will provide 64 ASCII chars, but we planned to use only 62 of them
5. Take only the first 7 characters - This is the length of the URL we planned to use.
    
