-- Schema and data manipulation scripts for Book Catalog

-- 1. CREATE TABLE Scripts
CREATE TABLE Authors (
    Id INT PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL
);

CREATE TABLE Books (
    Id INT PRIMARY KEY,
    Title NVARCHAR(300) NOT NULL,
    AuthorId INT NOT NULL,
    PublicationYear INT NOT NULL,
    CONSTRAINT FK_Books_Authors FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
);

-- 2. INSERT Script
INSERT INTO Authors (Id, Name) VALUES (1, 'Robert C. Martin');
INSERT INTO Authors (Id, Name) VALUES (2, 'Jeffrey Richter');

INSERT INTO Books (Id, Title, AuthorId, PublicationYear) VALUES (1, 'Clean Code', 1, 2008);
INSERT INTO Books (Id, Title, AuthorId, PublicationYear) VALUES (2, 'CLR via C#', 2, 2012);
INSERT INTO Books (Id, Title, AuthorId, PublicationYear) VALUES (3, 'The Clean Coder', 1, 2011);

-- 3. UPDATE Script
UPDATE Books SET PublicationYear = 2013 WHERE Id = 2;

-- 4. DELETE Script
DELETE FROM Books WHERE Id = 3;

-- 5. SELECT Script
SELECT b.Title, a.Name AS AuthorName
FROM Books b
JOIN Authors a ON a.Id = b.AuthorId
WHERE b.PublicationYear > 2010;


