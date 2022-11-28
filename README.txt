Asticom Backend Exam

A basic user management system API implementing authentication (login page), CRUD functions

**** PROJECT REQUIREMENT ****
- Visual Studio
- MSSQL Server

**** RUNNING THE PROJECT ****
- To be able to run the project
(1) You must setup the username and password for the MySQL connection.
Open the solution (.sln), and navigate to the Asticom_BackendExam project. Open the appsettings.Development.json file and update the username and password for you to establish the local connection.

(2) Create the database and tables for the project.
Migrations are already added on the project. In order to apply this on your local database, Go to Tools -> Nuget Package Manager -> Package Manger Console. Run the command: Update-Database

** Note: Please make sure that the Web API is selected as startup project before running the command, otherwise, it will not perform the operation.

**** HOW TO USE THE PROJECT ****
- Upon running the application, the swagger file will be loaded. In order to use the API endpoints for the UserController, you need to add the authentication by requesting for the Bearer token on the Admin controller ([POST]api/admin/token). The default admin account has been seeded on the project. You can find it on the ("/Models/Seeder.cs"). You should be getting the token after successfully sending the correct credentials. Click the 'Authorize' button to submit the Bearer token.

For reference, here is the credentials you can use to login:
kevin_admin@sample.com
P@ssword!1

**Note: Some API endpoints has it's validation rule. If any input is invalid, you will get an error message included on the response.

**** LIMITATIONS ****
- The password is not available as parameter request, instead a ramdom generated characters will be provided and be saved on the database.
- The admin cannot also see the password as part of the User Response. This is to prevent the unauthorized access of user accounts.
- The user details (username and password) is not saved on the Identity Table and only the admin account is stored there.

