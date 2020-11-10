Feature: Search

Scenario: Track can be searching by title
Given All data scripts
When searching for Coaster
Then the following tracks are found:
| Id   | Title          | Artist     | Recorded Year |
| 3567 | Yakety yak     | Coasters   | 1958          |
| 4444 | Roller Coaster | Danny Vera | 2019          |

Scenario: Tracks can be searching by artist
Given All data scripts
When searching for Soleil
Then only the track Alegria by Cirque du Soleil recorded in 1997 is found

Scenario: Track can be searched by their full RecordedYear
Given All data scripts
When searching for 1954
Then only the track White Christmas by Bing Crosby recorded in 1954 is found

Scenario: Searching track by their RecordedYear are only found by searching the full year
Given All data scripts
When searching for 54
Then the track White Christmas is not found