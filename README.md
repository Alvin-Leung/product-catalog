# Dynamic Product Catalog

This is an example full-stack application featuring a React/Vite frontend, a C# ASP.NET Core backend, and a SQLite database. It showcases an implementation of debounced search on the frontend, and full-text search on the backend.

# Features

Below is a summary of features already implemented for this example application.

**Frontend**
- Dynamic search of backend products as you type
- Debounced search to prevent overloading the backend
- Efficient loading and display of 1000s of items from backend

**Backend and Database**
- Convenient test data generation endpoint for populating database with example product test data
- Database
  - Full-text search design for efficient dynamic search 
  - Quick database setup via Entity Framework
  - Minimal database dependencies with SQLite
 
# Future Improvements

Below are features that could greatly improve this example application. If you are using this full-stack application for educational purposes, the features below could be great to implement as an exercise:

**Frontend**
- Adjustable column width
- Highlight found search terms
- Column visibility selection
- Sort by column
- Filter on specific columns
- Total result count

**Backend**
- Return paged results
- Better error handling
- Logging

**General**
Setting up and running app is not easy right now. This could be improved with
- Scripting to simplify setup of frontend and backend
- OR Containerizing frontend and backend to remove the need for installation of dependencies
 
# Setup and Run Instructions (Local)

To get this application up and running on your local machine, execute the steps below.

## 1. Prerequisites

- .NET SDK: Ensure you have the .NET 8 SDK (or later compatible version) installed. You can download it from https://dotnet.microsoft.com/download/dotnet/8.0
- Node.js & npm: Install Node.js (which includes npm) from https://nodejs.org/
- Git: Make sure Git is installed to clone the repository

## 2. Clone Repository

```shell
git clone https://github.com/Alvin-Leung/product-catalog.git
```

## 3. Backend Setup & Run

a. Navigate into the backend directory

```shell
cd backend/Api
```

b. Restore NuGet packages

```shell
dotnet restore
```

c. Create the database using Entity Framework

```shell
dotnet ef database update
```

d. Run the backend application:

```shell
dotnet run
```

e. [Optional] Check out the backend swagger page at https://localhost:7120/swagger/index.html

## 4. Frontend Setup & Run:

a. Open a new terminal window and navigate to the frontend directory

```shell
cd frontend/client
```

b. Install frontend dependencies

```shell
npm install
```

c. Run the frontend development server

```shell
npm run dev
```

e. Note the URL indicated by the above command

## 5. Populate Test Data

Run the following curl command to create test product data:

```shell
curl -X 'POST' \
  'https://localhost:7120/api/Products/generate' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "numProductsToGenerate": 100
}'
```

`numProductsToGenerate` can be set to your desired number. Alternatively, this can be done via the backend swagger page at https://localhost:7120/swagger/index.html.

## 6. Try the App

Open a browser and navigate to http://localhost:5173/ to check out the app :)
