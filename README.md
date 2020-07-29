#RMIS Demo API
=====================

Database-First Approach
------------------------
Create the database and tables in Azure SQL Database.
Install these packages:
```
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore.SqlServer
```
Eventually Newtonsoft JSON package will be needed to parse JSON objects for Controllers:
```
Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson -Version 3.1.6
```

Scaffold the Models
-----------------------
Create the models from the existing database using Scaffold-DbContext command.

The Server name is <SERVER_NAME>.windows.net for Azure SQL Database.

The Database name is whatever the name of the DB is.

You will also need a Username and Password that is created for the specific Azure SQL Server.
Running this command will create a Models folder in the project solution:
```
Scaffold-DbContext "Server=<HOSTED_SERVER_NAME>.windows.net;Database=<DATABASE_NAME>;Trusted_Connection=False;Encrypt=True;User ID=<USERNAME>;Password=<PASSWORD>;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```
Now the Models and Data Context exist.

