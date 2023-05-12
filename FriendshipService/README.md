# Friendship Service
Purpose of this service is to handle friendship network between users.

## Functional Requirement

1. Add Friends
2. View Friends
3. Remove Friends - TODO
4. View Mutual Friends of a Friend - TODO

## Non-Functional Requirement

1. Low Latency
2. High Availability

## Implementation consideration

As previous we can have millions of requests hitting our service per second. For example, let's say there is a viral post going on by some user, 
and the other users gonna view that post immediately and gonna check the authors profile, which gonna see the friends list of that author. 
If 1M users gonna view that profile same time, that gonna have huge traffic to our service. Plus that kind of traffic is highly unpredictable. 
So, our service should be highly-available (NFR #2) and I don't want to worry about adding scaling rules to scale our service based on that unpredictable traffic. 
So, I'm picking azure functions here cause that will remove the burden of worry about scaling and it's highly available.

And as the database we could pick relational database to store this friendship relationship as Many to Many table. But consider a high profile user with 1M friends. 
In such case, that would be very hard to query Friends of his Friends, Mutual Friends, etc. Cause we have to do lot's of joins and that could be expensive and slow. 
Which could break our NFR #1. Because of that we would pick graph database as our datastore. 
Because graph databases are the best fit on storing data model which has relationships between data and we need to query those relationships. Such as social networking, recommendation engine, etc.
For this we will be using Azure CosmosDB for Apache Gremlin Account API, which has graph database service and it's a Globally-distributed NoSQL database service.

## Add Friends Feature

![LikeService (2)](https://github.com/DHJayasinghe/system-design/assets/26274468/ca84afce-1ae6-4814-a6b8-d0a2fbce4ec4)

When we need to add a friend. Let's say adding Michael as a friend of John. I'll will be first checking whether there is already **Person** vertices available for John & Michael. 
If so, wil get those vertices, if not will create them. Then we gonna create an **edge** on John Person vertice pointing Michael Person vertice.

## View Friends Feature

![LikeService (1)](https://github.com/DHJayasinghe/system-design/assets/26274468/389f4ce9-2434-412a-bd4f-cfd0750bd711)

When comes to viewing friends of William, we will start with Vertice with label 'Person' and UserId = 'William' and gonna use both() gremlin step, which gonna navigate to Ingoing and Outgoing vertices from current Vertice.
And return the result. Which gonna output me the Friends of William.

