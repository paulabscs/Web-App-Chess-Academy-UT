**paulabscs**

# Prototype Distributed End-To-End Web App

This project represents an evolving distributed end-to-end web app utilizing C# integrated with core technologies: Entity Framework Core, ASP.NET Core, and .NET framework.

## Architectural Overview

### Server
This project builds on a standard Model-View-Controller approach and a distributed microservices model. Session handling mechanisms ensure standard coverage of authentication and authorization - key aspects of any web app. A database is utilized for persistent data storage, complemented by a repository pattern to facilitate content delivery to the end user. 

### Client
Incorporated distributed services are designed to utilize OOP principles, specifically adhering to the Don't Repeat Yourself principle. Reusable methods for different API endpoints ensure stable messaging from the back-end to the user-friendly, responsive client-side of the user management service.

### Structure

-Database Layer: /Data
-ORM Layer: /Dtos
-Model Layer: /Models
-Controller Layer: /Controllers
-View Layer: /wwwroot

## Endpoints

### Reactive to User Actions

- ListItems (GET): Retrieves items
- Comment (POST): Allows posting comments
- Register (POST): Onboards new users
- Login (POST): Authenticates credentials
- Logout (GET): Terminates session

### Execution not directly tied to user actions
- GetVersion (GET): Retrieves version from the database
- GetItemPhoto/{id} (GET): Delivers item photos
- Session (GET): Provides session details
- Comments (GET): Fetches comments

## Usage Information
- VSCode friendly, flexibility in how to build and run this project is apparent
- Open terminal & change directory to the desired folder containing this project. Use the commands "dotnet build" & "dotnet run"
- Navigate to "https://localhost:5283" or, if using a different port, modify the files: UT.http, launchSettings.json & authentication.js





