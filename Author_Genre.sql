create database MangaDB;
go
use MangaDB;

CREATE TABLE Genre (
    GenreId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL
);

CREATE TABLE Author (
    AuthorId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL
);

