# MiniGig-ASP.NET-MVC5
MiniGig ASP.NET MVC 5 is a web based application created in Visual Studio 2017 Professional and C# .NET 4.7.2.

The motivation was to upgrade MiniGig Web App using ASP.NET MVC 5, Code First with Entity Framework 6.2.0 using localdb and testing code without hitting the database with the test mock support introduced in EF6.


The solution contains eight projects:
* Data
	* MvcMiniGigApp.Data
* Domain
	* MvcMiniGigApp.Domain
* Services
	* MvcMiniGigApp.Services
* Tests	
	* MvcMiniGigApp.Data.Tests
	* MvcMiniGigApp.Services.Tests
* MvcMiniGigApp.Web
* SharedKernel
* SharedKernel.Data


Use Cases:
*   Display list of gigs paged
    ![picture alt](https://github.com/monicacrespo/MiniGig-ASP.NET-MVC5/tree/master/MvcMiniGigApp.Web/Images/DisplayNumberOfGigsPerPage.JPG)
*   Create a gig
    *   Gig identifier, name, date, genre of music
    *   gig identifier automatic generated
*  Edit the specified gig by its identifier
   ![picture alt](https://github.com/monicacrespo/MiniGig-ASP.NET-MVC5/tree/master/MvcMiniGigApp.Web/Images/EditGig.JPG)

   ![picture alt](https://github.com/monicacrespo/MiniGig-ASP.NET-MVC5/tree/master/MvcMiniGigApp.Web/Images/EditGigDatePicker.JPG)

*  Details of the specified gig by its identifier
*  Remove the specified gig by its identifier

The code illustrates the following topics:

* Encapsulation of EF using GenericRepository for CRUD operations so that the UI does not have any direct interaction with the entity
* Implementation of DI with StructureMap 4.5.3
* Improving the user experience using JQuery, DatePicker JQuery UI widget and HTML5
* Implementation of paging using PagedList.Mvc 
* Testing code without hitting the database with the test mock support introduced in EF6
	* Testing query scenarios
	* Testing non-query scenarios
* Testing Generic Repository code


# Getting Started
I've included logic in to create and seed a database using Code First migrations.
When you run the application and it accesses the database for the first time, Code First uses the MigrateDatabaseToLatestVersion initializer class to check if the database matches the data model. If there's a mismatch, Code First 
* automatically creates the database (if it doesn't exist yet) or 
* updates the database schema to the latest version (if a database exists but doesn't match the model).

The application implements a Migrations Seed method, so that the method runs after the database is created or the schema is updated. The Migrations Seed method inserts fictional gigs in the development database LocalDB.


Software dependencies
  1. .Net Framework 4.5 or higher
  2.  SQL Express LocalDB (v13.0) that is a version of SQL Express created specifically for developers.
