Time to Level Up: The Upgaming Tech
Challenge
Welcome! Upgaming
Thank you for your interest in our .NET & SQL Developer Acceleration Program! This technical task is
designed to assess the foundational skills required to succeed in the program. It allows you to
showcase your current knowledge of C# and web basics. We are looking for candidates with strong
potential and a passion for learning, so please do your best. We're excited to see your work!
A Note on Using AI Tools
Modern development involves using all the tools at your disposal, and that includes AI assistants like
GitHub Copilot or ChatGPT. We encourage you to use them as a resource to help you learn and build,
just as you would use official documentation or Stack Overflow.
The goal of this task is for us to understand how you solve problems. So, while you can use these tools
for help, make sure you can still:
Explain the code you've written and the choices you made.
Walk us through your thought process for a particular solution.
Think of it this way: the final solution should be something you feel ownership of and can confidently
discuss. That's what we're excited to see!
Task Overview
Your assignment is to build a simple "Book Catalog" REST API. The task is divided into two core parts and one
optional bonus part.
Part 1: .NET Minimal API: You will create an API to manage Authors and their Books . To keep things
simple, you will use simple C# Lists to store your data directly in the code—no actual database connection
is required for the C# part.
Part 2: SQL Scripts: You will write the fundamental SQL scripts to create and populate Authors and
Books tables that correspond to your API's data model.
Part 3 (Optional): You can extend the API's functionality to showcase your skills further.
Technical Requirements
ASP.NET Core (Minimal API is recommended for simplicity)
.NET 6 is recommended, as it will be used in the program. However, solutions using .NET 5 or
.NET Core 3.1 are also perfectly acceptable.
C#
Your preferred IDE (Visual Studio 2022 or VS Code is recommended)
Git for version control
✓ Part 1: Core Task - Book Catalog API
1. Project Setup
Create a new ASP.NET Core project. We recommend using the Minimal API template for simplicity.
2. Model Creation
Create the following classes to represent your data and shape your API's responses.
Author.cs
public class Author
{
public int ID { get; set; }
public string Name { get; set; }
}
Book.cs
public class Book
{
public int ID { get; set; }
public string Title { get; set; }
public int AuthorID { get; set; } / Foreign Key
public int PublicationYear { get; set; }
}
Pro Tip: It's a common best practice to use a "Data Transfer Object" (DTO) to shape the data
you send back from your API. This gives you control over what the client sees.
BookDto.cs (Response Model)
public class BookDto
{
public int ID { get; set; }
public string Title { get; set; }
public string AuthorName { get; set; }
public int PublicationYear { get; set; }
}
3. In-Memory Data Store
In your Program.cs file or a separate static class, create two static List<T> collections to act as
your data store for Authors and Books . Pre-populate the lists with at least 2 authors and 3-4
sample books, ensuring the AuthorID in the books corresponds to an author.
/ Example data store
private static List<Author> _authors = new List<Author>
{
new Author { ID = 1, Name = "Robert C. Martin" },
new Author { ID = 2, Name = "Jeffrey Richter" }
};
private static List<Book> _books = new List<Book>
{
new Book { ID = 1, Title = "Clean Code", AuthorID = 1, PublicationYear = 2008
},
new Book { ID = 2, Title = "CLR via C#", AuthorID = 2, PublicationYear = 2012
},
new Book { ID = 3, Title = "The Clean Coder", AuthorID = 1, PublicationYear =
2011 }
};
4. API Endpoints
Implement the following three (3) endpoints:
A. Get All Books (with Author Name)
GET /api/books
Functionality: Returns a complete list of all books. Crucially, the response should be a list of
BookDto objects. You will need to combine data from your two static lists to populate the
AuthorName field.
B. Get Books by a Specific Author
GET /api/authors/{id}/books
Functionality: Returns a list of all books written by the author whose ID is specified in the URL. If no
author with the given ID exists, it should return a 404 Not Found response.
C. Add a New Book
POST /api/books
Functionality:
Accepts a new book's details ( Title , AuthorID , PublicationYear ) in the request body.
Generates a new unique ID for the book.
Adds the new book to the static list.
Returns the newly created book object (including its new ID ) with a 201 Created status code.
5. Basic Validation
In the POST /api/books endpoint, add the following validation rules:
The Title cannot be null or empty.
The PublicationYear cannot be in the future.
The AuthorID provided must correspond to an existing author in your _authors list.
If validation fails, the API should return a 400 Bad Request response with a meaningful error
message.
✓ Part 2: Core Task - SQL Scripts
Important Note
Before you begin this section, please review the "Database Submission (Choose One Option)"
section at the end of this document. This will help you decide whether you want to submit your
SQL work as a database backup (.bak) file or as a single script file (schema.sql). Knowing your
end goal will help you approach these tasks more efficiently.
For this part, you will provide all the necessary SQL code to create, populate, and manage your
database.
SQL Task List
1. CREATE TABLE Scripts: Provide scripts to create both the Authors and Books tables, ensuring
their structure matches the C# models you created in Part 1. Remember to define a
PRIMARY KEY for each table and a FOREIGN KEY relationship between them.
Pro Tip: Two Ways to Create Your Scripts
You have two great options for generating the CREATE TABLE scripts:
Code-First: Write the CREATE TABLE statements manually in the .sql file. This is a
great way to show your T-SQL knowledge directly.
UI-First (Recommended for Beginners): Use the graphical interface in a tool like SQL
Server Management Studio (SSMS) to design your tables. Once you've saved your
tables, you can right-click the table > Script Table as > CREATE To > New Query Editor
Window. This will automatically generate the correct script for you, which you can then
copy into your schema.sql file.
A Note on Data Manipulation Scripts (Tasks 2-5)
For the next four tasks that involve handling data ( INSERT , UPDATE , DELETE , and
SELECT ), you have the flexibility to choose your preferred approach. You can either:
Option A (Standard Approach): Provide the direct SQL statements (e.g., INSERT ,
UPDATE , DELETE , SELECT ).
Option B (Advanced Approach): Create a Stored Procedure for each operation.
You do not need to do both. Simply choose one consistent method for these tasks and
provide the corresponding scripts.
2. INSERT Script: Provide a script to insert your sample authors and books into the tables.
3. UPDATE Script: Provide a script to update the PublicationYear of a specific book (e.g., set the
year to 2013 for the book with ID 2 ).
4. DELETE Script: Provide a script to delete a specific book by its ID (e.g., delete the book with
ID 3 ).
5. SELECT Script: Provide a script to retrieve all book titles and their author's name for books
published after 2010 .
Part 3: Bonus Task OPTIONAL
A Note on Quality vs. Quantity
This section is completely optional. Our primary evaluation will be based on the quality of your
work in Part 1 and Part 2. We would much rather see a polished, well-executed solution for the
core tasks than a rushed attempt at all three parts.
Please focus on making the core tasks perfect first. Only attempt this section if you have
completed the main requirements to a high standard and have extra time. Please choose and
complete only one of the three options below.
Choose ONE of the following options:
Option A: Advanced Filtering & Sorting
GET /api/books?publicationYear={year}&sortBy={field}
Functionality: Enhance the GET /api/books endpoint to support both filtering by
publicationYear and sorting by title .
Example Requests:
GET /api/books?publicationYear=2008 - Returns only books from that year
GET /api/books?sortBy=title - Returns all books sorted alphabetically by title
Expected Response: Same as Part 1 (list of BookDto objects), but filtered/sorted based on
query parameters.
Option B: Robust Update Endpoint
PUT /api/books/{id}
Functionality: Update an existing book's details. The logic must include validation to ensure
the updated AuthorID (if provided) corresponds to a valid author.
Request Body: Book object with updated fields
Response Codes:
204 No Content - Success
404 Not Found - Book doesn't exist
400 Bad Request - Validation fails
Option C: Get Author with Nested Book List
GET /api/authors/{id}
Functionality: Return a single author's details along with a nested list of all their books.
Response Format: You will need to create a new DTO for this response (e.g.,
AuthorDetailsDto with a List<Book> property).
Response Codes:
200 OK - Success with author and books
404 Not Found - Author doesn't exist
What We're Looking For
Correctness: Does your application meet all the requirements and function as expected?
Code Quality & Comments: Is your code clean, readable, well-organized, and appropriately commented?
Clear comments on complex logic are a plus.
Problem-Solving: How did you approach modeling the data relationship and combining the data in your
C# code?
Graceful Error Handling: Does the API handle invalid inputs and non-existent IDs correctly?
Thoughtful Data Shaping: Did you effectively use the BookDto to craft a clean API response?
Git Usage: A clear history of commits is a plus.
Submission Guidelines
1. Create a new public repository on GitHub (upgaming-firstname-lastname).
2. Commit your complete solution. Make sure to include your SQL submission (either the .bak file
or the schema.sql file).
3. Follow the database submission instructions below.
4. Email the link to your public GitHub repository.
Database Submission (Choose One Option)
Please Note: Both options below are equally valid and will be evaluated in exactly the same way. Your
choice will not affect your assessment. Please choose the option that is most convenient for you
based on your local development setup.
Option 1 (Recommended): Database Backup File
If you created a local MS SQL database to test your scripts, the best way to share it is with a
backup .bak file.
How to create a .bak file in SSMS:
1. Right-click on your database > Tasks > Back Up....
2. Ensure the "Backup type" is Full.
3. Note the destination path and click OK.
4. Create a folder named database in your GitHub repository and place this .bak file
inside it.
Option 2 (Alternative): SQL Script File Only
If you are unable to provide a database backup, submitting a single, complete, and runnable
schema.sql script is perfectly acceptable. Please ensure this single file contains all the
required SQL tasks ( CREATE , INSERT , UPDATE , DELETE , SELECT ).
Good luck!