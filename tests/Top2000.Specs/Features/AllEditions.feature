Feature: All Editions

Scenario: The first edition of the Top2000 was in 1999
Given All data scripts
When the feature is executed
Then the latest year is 1999

Scenario: Every edition is returned starting with the highest year
Given All data scripts
When the feature is executed
Then a sorted set is returned started with the highest year

Scenario: An Edition contains the year start and end date of the Top2000 for that year
Given All data scripts
When the feature is executed
Then For each edition the start and end datetime matching that editions year

Scenario: An Edition contains the datetime in local time
Given All data scripts
When the feature is executed
Then the Start- and EndDateTime is in local time