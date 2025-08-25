# How to Run

Running the project in **Visual Studio** is straightforward:

1. Build both projects by right-clicking on each folder and selecting **Build**.  
2. Run the solution.
> **_NOTE:_** You may need to update or create the database if you haven't done so already.

## Environment Setup
Before running the project, you need to create environment files.  This project will not work without these files.

1. Create `.env.development` and `.env.production` in the `nccsop.client` folder.  
2. Copy the contents of `.env.example` into each file.  

   You can do this manually, or via the command line:

   ```bash
   cp .env.example .env.development
   cp .env.example .env.production
   ```
 3.  The client_id and client_secret can be found in the 1Password.

## If not using Visual Studio
- Frontend
   - Navigate to the nccsop.client folder and run:
   - `npm run build`
- Backend
   - Navigate to the NCCSOP.server folder and run:
   - `dotnet build`
- Database
   - Update or create the database using Entity Framework:
   - `dotnet ef database update`
  > **_NOTE:_**  SQL Server or SQL Express is required.
- Run both the backend and frontend projects.

## Authentication notes
- The project uses OAuth 2.0 Authorization Code Flow with OpenID Connect.
- If you do not have an Identity Provider (IdP), you can either:
    - Update authService.ts to point to a valid authority, or
    - Omit authentication entirely for testing purposes. To omit, go to `/nccsop.client/router/index.ts` and comment out the `meta: { requiresAuth: true }` lines.
