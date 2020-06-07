CREATE TABLE Track
(
    Id int NOT NULL,
    Title nvarchar(100) NOT NULL,
    Artist nvarchar(100) NOT NULL,
    Year int NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE PlayTime
(
    TrackId int NOT NULL,
    Year int NOT NULL,
    DateAndTime datetimeoffset NOT NULL,
    PRIMARY KEY (TrackId, Year),
    FOREIGN KEY (TrackId) REFERENCES Track (Id)
);

CREATE TABLE Position
(
    TrackId int NOT NULL,
    Year int NOT NULL,
    Position int NOT NULL,
    PRIMARY KEY (TrackId, Year),
    FOREIGN KEY (TrackId) REFERENCES Track (Id)
);