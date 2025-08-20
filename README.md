# How to run
This is all pretty simple in Visual Studio just build both projects (right click both folders and click "Build") then run.

If not using Visual Studio
- Build the frontend by going into the `nccsop.client` project and running `npm build`
- Build the backend by going into the `NCCSOP.server` project and running `dotnet build`
- run the backend and the frontend projects.
- note: inside `authService.ts`, you will need to change the authority or just omit authentication overall if you don't already have an idp.
    - authentication uses OAuth 2.0 Authorization Code Flow with OpenID Connect.
