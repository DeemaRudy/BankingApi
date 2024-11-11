# BankingApi

Instructions for Running the Project in Visual Studio

1. Clone the Repository
1.1 Open Visual Studio.
1.2 Copy repository link 'https://github.com/DeemaRudy/BankingApi'
1.3 Go to File > Clone Repository, and paste the copied URL to clone the repository.\
2. Install Required Tools
2.1 .NET SDK: https://dotnet.microsoft.com/download.
3. Restore Project Dependencies
3.1 In Visual Studio, once the project is loaded, Visual Studio will automatically try to restore all the required NuGet packages. 
3.2 If it does not, you can manually restore them: Right-click on the solution in Solution Explorer and click Restore NuGet Packages.
4. Apply Database Migrations:
Open Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console).
Run the following command to update the database schema:
'dotnet ef database update'
This will create the SQLite database and apply migrations that are already defined in the project.
(Please note that Entity Framework is located in the BankingApp.DAL project, so the command needs to be executed for that project.)
5. Run the Application
In Solution Explorer, right-click on the project that contains the API (BankingApp.API) and select Set as StartUp Project.
Click Start or press F5 to run the application.