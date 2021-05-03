# EmployeeDetails-with-WCF-RestService

Technologies used - C# ,ASP.net version 4.5,WCF Rest Service,Entity Framework,MSSQL, JavaScript, HTML5 and CSS. Created a Role-based authorization to a C#  MVC  application. Roles and authorizations are as bellow

Admin -  Admin can Create, View details, Delete, Delete Multiple employees, and Edit an Employee.
Manager - The manager can Create, View details and Edit Employees.
Employee - The Employee can only View details.
If someone signs up with a different name, he will be given an Employee role.  All Roles can search  Employees with their Name and Gender.  All Roles can sort Employees by clicking on Name and Gender.

Signup using the following data on the signup page. The password is hashed and stored in the UserTable. 

Admin - Username - Admin Password - admin

Manager - Username - Manager Password - manager

Employee - Username -Employee Password - employee

Database Details

1.CREATE TABLE [dbo].[UserTable]( [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [UserName] nvarchar NULL, [Password] nvarchar NULL)

2.CREATE TABLE [dbo].[UserRole]( [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [UserId] [int] NOT NULL, [Role] nvarchar NOT NULL)

ALTER TABLE [dbo].[UserRole] WITH CHECK ADD CONSTRAINT [fk] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserTable] ([Id]) ON DELETE CASCADE GO

ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [fk] GO

3.Insert into UserRole Values(1,"Admin") Insert into UserRole Values(1,"Manager") Insert into UserRole Values(1,"Employee") Insert into UserRole Values(2,"Manager") Insert into UserRole Values(3,"Employee")
