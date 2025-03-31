CREATE TABLE Manga (
    MangaId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Description TEXT,
    CoverImage NVARCHAR(1000),
    BackgroundImage NVARCHAR(1000),
    Status NVARCHAR(50),
    UserId INT,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Chapter (
    MangaId INT,
    ChapterId INT,
    Title NVARCHAR(255) NOT NULL,
    Price DECIMAL(10,2) DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (MangaId, ChapterId)
);

CREATE TABLE Content (
    MangaId INT,
    ChapterId INT,
    ContentId INT,
    Image NVARCHAR(1000),
    PRIMARY KEY (MangaId, ChapterId, ContentId)
);
