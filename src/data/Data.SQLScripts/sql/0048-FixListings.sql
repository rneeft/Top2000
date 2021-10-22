-- Merge duplicate listing: Biko - Peter Gabriel.
UPDATE Listing 
SET TrackId = 347 
WHERE TrackId = 3916;

DELETE Track WHERE Id = 3916;

-- School Supertramp is in the wrong play time.
UPDATE Listing 
SET [PlayUtcDateAndTime] = '2020-12-31T17:00:00' 
WHERE TrackId = 2601 AND Edition = 2020;

