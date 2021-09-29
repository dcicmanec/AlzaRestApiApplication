# AlzaRestApiApplication
Alza Rest Api Web .Net Application

Solution consist of 5 individual projects

1. AlzaRestApiApplication - Main front-end Web .NET application.
2. ShoppingItems.Models - Class Library representing Rest Api prifile.
3. ShoppingItems.Core - Class Library representing infrastructure of Rest Api, included Data Access Layer (DAL) and Business Layer (BL).
4. ShoppingItems.Database - SQL Server Database project representing Structure and Data of simple ShoppingItems database.
5. AlzaRestApiApplication.Tests - Project used for Unit testing

Configuration
- Please modify appSettings 'RestApiHost' parameter in Web.config <add key="RestApiHost" value="http://localhost:63895" /> for main AlzaRestApiApplication project
- Please modify appSettings 'RestApiHost' parameter in App.config <add key="RestApiHost" value="http://localhost:63895" /> for AlzaRestApiApplication.Tests project

Rest Api Swagger documentation can be downloaded from link:

- https://app.swaggerhub.com/apis-docs/test62993/AlzaRestApi/1.0.0
- Documentation file can be accessed also in running application under link in main menu (/Swagger).

Solution is awailable and can be downloaded from GitHub.com

- https://github.com/dcicmanec/AlzaRestApiApplication

Data layer

- SQL Server Database can be fully restored from ShoppingItems.Database project.
- SQL file with database Structure and Data is located in App_Data folder.
- Database connection string is stored in Web.config under name 'ShoppingItemsConnectionString'.

Paging configuration

- To change number of records displayed on one page you can modify appSettings parameter 'PageSize' <add key="PageSize" value="10" /> in Web.config


