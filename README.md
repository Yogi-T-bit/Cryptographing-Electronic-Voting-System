# Cryptographing Electronic Voting System

# DB Migration
- To create a new migration and update the database, run the following commands in the terminal:
```DotNet
dotnet ef migrations add <Migration Name> --project DataAccess_Project --startup-project WebApp_Project
dotnet ef database update --project DataAccess_Project --startup-project WebApp_Project
```
- Example:
```DotNet
dotnet ef migrations add InitialCreate --project CryptographingElectronicVotingSystem.Dal --startup-project CryptographingElectronicVotingSystem.Web
dotnet ef database update --project CryptographingElectronicVotingSystem.Dal --startup-project CryptographingElectronicVotingSystem.Web
```