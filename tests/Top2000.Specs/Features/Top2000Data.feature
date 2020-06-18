Feature: Top2000Data

Background: Data for the Top2000 app is stored in SQL scripts inside the `Data.SQLScripts` assembly. This assembly is then used in the `Data.ClientDatabase` project which creates the client database. 

On the offcial Top2000 website (www.top2000.nl) people can choose their top tracks for the Top2000. After the voting week the first 10 tracks are published. A few days before the show starts the full list is published. 

Scenario: All editions, except for the last, contains 2000 positions starting with 1 and ending with 2000
Given all data scripts
When the client database is created
Then except for the last edition, the listing table contains 2000 tracks for each edition ranging from 1 to 2000

Scenario: The last edition can either have 10 or 2000 tracks
Given all data scripts
When the client database is created
Then the listing table contains 10 or 2000 for the last edition ranging from 1 to 10/2000

Scenario: The playtime of each track is either the same to the last track of increment by one hour
Given all data scripts
When the client database is created
Then for each track in the listing table the PlayDateAndTime is the same to the previous track or has incremented by one hour