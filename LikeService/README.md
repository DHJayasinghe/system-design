# Like Service
Purpose of this service is to handle users Reactions on Posts and their Comments.

## Functional Requirement

1. Add Reaction - Users should be able to put a reaction on Post and It's comment. Should allow multiple reaction types (ThumbsUp, Heart, Care, Laugh, Sad, Angry)
2. View Reaction count - User should be able to see the reaction counts of Post and it's Comments seperately.
3. Remove Reaction - Users should be able to remove Reaction they added
4. Only one reaction should allow for a Post or a Comment from one user.
5. User should be able to change the reaction type they provided.

## Non-Functional Requirement
1. Low Latency - There can be millions of reactions per post, Calculating count of reactions should be fast. 
2. High Availability - There can be millions of reactions per second hitting our service. Therefore our backend service should be highly available.
3. Consistency - Showing actual reaction count does not required to be real-time. Having a lag on showing it, is Okay. Stronger consistency not required.
