# WookieBooks

## Objective
To implement a bookstore REST API using C# and .NET Core.

## API METHODS
* GET /Books 
    * fetches all the book records
* GET /Books/{id}
    * fetch the book records with the requested id
* POST /Books
    * add new book record
* PUT /Books
    * edit existing book record
* DELETE /Books{id}
    * delete the book record with the requested id

## Technology Stack
* Language - C#
* Framework - .NET Core 3.1
* Database - In-Memory database
* Logger - Log4net
* Swagger included
* Unit Test using xUnit
* Postman collection also included

## To run the application
    * Using dotnet cli  
    **1.** Open command prompt and navigate to the solution folder 
    **2.** dotnet run 
    **3.** In case you get error on dotnet run related to dev cert : dotnet dev-certs https --trust 
    **4.** View swagger at https://localhost:58252/swagger/index.html OR you may check using Postman.
     
    Or
    
    * Using Visual Studio 
    **1.** Open solution in Visual Studio 
    **2.** Run using IIS Express 
    **3.** Swagger will open in the browser URL: https://localhost:58252/swagger/index.html OR you may check with Postman 
    
    You can find logs in bin folder. Logs are being logged in INFO level.

