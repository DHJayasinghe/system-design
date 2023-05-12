# Facebook System Design

This repository is about Facebook system design on core functionalities using Azure technologies.

## Functional Requirements
1. Post (Text + Image + Video) - Should be able to do a Post with a Video or Image
2. Add Friends - Should be able to Add other users as Friends
3. Like & Comment - Should be able to Like and Comment Posts. Should see how much comments and Likes count on my posts. Also view the comments.
4. Timeline - Should be able to see the timeline. If I have 100 friends, if they Posts something, I should see that on my Timeline. Also, posts I have made. This should be in reverse chronological order (recent comes first).


## Non-Functional Requirements
1. Optimized for Read Heavy workload - Social platforms are very read heavy. Cause if I post something and I have 100 friends, then there will 100 reads for that post.
2. Latency - We should have fast rendering on Posts, Profiles, Timeline etc.
3. Consistency - Having a lag on showing latest Posts and Timeline is Okay. It does not have to me real-time like chatting. If a friend post something and it takes about 30sec or 1min to show up on my Timeline, that is fine. So, no strong consistency is required.
4. Access Pattern - When user post something there can be heavy access on that Post for the first couple of days, weeks and months. But after that there won't be any activity on that post. We can use that to maintain the cost on storage of accessing the content (Images, Videos, etc)
5. Global User Access - The platform can have users coming from lot's of countries from the Global. So, the platform should be distributed across the globe for fast access.
6. Scale - Facebook has<br>
&emsp;- Aprx. 2B daily active users (DAU) and 3B monthly active users (MAU)
&emsp;- Aprx. 98% of them access the platform using mobile - We should consider this on optimizing images and videos for fast rendering
&emsp;- Aprx. 200K photos uploaded every minute, and 230K likes and comments every minute - We should consider these volune stats on deciding optimizations like caching.
