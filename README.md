# Dynamic Product Catalog

This is a full-stack application featuring a React/Vite frontend, a C# ASP.NET Core backend with Entity Framework, and a SQLite database.

# Setup and Run Instructions (Local)
To get this application up and running on your local machine, follow these steps:

1. Prerequisites:

- .NET SDK: Ensure you have the .NET 8 SDK (or later compatible version) installed. You can download it from https://dotnet.microsoft.com/download/dotnet/8.0.
- Node.js & npm: Install Node.js (which includes npm) from https://nodejs.org/.
- Git: Make sure Git is installed to clone the repository.

2. Clone the repository

3. Backend Setup & Run:

a. Navigate into the backend directory:

Bash

```shell
cd backend/Api
```

b. Restore NuGet packages:

Bash

```
dotnet restore
```

c. Create the database with the following:

Bash

```
dotnet ef database update
```

d. Run the backend application:

Bash

```
dotnet run
```

4. Frontend Setup & Run:

a. Open a new terminal window and navigate to the frontend directory:

Bash

```
cd frontend/client
```

b. Install frontend dependencies:

Bash

```
npm install
```

c. Run the frontend development server:

Bash

```
npm run dev
```

5. Populate data

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

numProductsToGenerate can be set to your desired number

6. Open a browser and navigate to the front end url to check out the app :)