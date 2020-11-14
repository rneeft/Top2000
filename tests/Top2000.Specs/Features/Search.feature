Feature: Search

Scenario: Track can be searching by title
Given All data scripts
When searching for Coaster
Then the following tracks are found:
| Id   | Title          | Artist     | Recorded Year |
| 4444 | Roller Coaster | Danny Vera | 2019          |
| 3567 | Yakety yak     | Coasters   | 1958          |

Scenario: Tracks can be searching by artist
Given All data scripts
When searching for Soleil
Then the following tracks are found:
| Id  | Title   | Artist           | Recorded Year |
| 119 | Alegria | Cirque du Soleil | 1997          |

Scenario: Track can be searched by their full RecordedYear
Given All data scripts
When searching for 1954
Then the following tracks are found:
| Id   | Title           | Artist      | Recorded Year |
| 3468 | White Christmas | Bing Crosby | 1954          |

Scenario: Searching track by their RecordedYear are only found by searching the full year
Given All data scripts
When searching for 54
Then the track White Christmas is not found

Scenario: Searching will have a result cap of 100
Given All data scripts
When searching for a
Then the results contain 100 items