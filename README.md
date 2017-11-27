# MiniGig-ASP.NET-MVC5
MiniGig ASP.NET MVC 5 is a web based application created in Visual Studio 2015 Professional and C# .NET 4.6.1.

The motivation was to upgrade MiniGig Web App using ASP.NET MVC 5, Code First with Entity Framework 6.2.0 using localdb and testing code without hitting the database with the test mock 
support introduced in EF6.


The solution contains six projects:
* MvcMiniGigApp.Data
* MvcMiniGigApp.Domain
* MvcMiniGigApp.Web
* SharedKernel
* SharedKernel.Data
* MvcMiniGigApp.Tests.Data

Use Cases:
*   Create a gig
    *   Gig identifier, name, date, genre of music
    *   gig identifier automatic generated
*  Edit the specified gig by its identifier
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
I've included logic in to create and seed a database to run the app.
 
Software dependencies
  1. .Net Framework 4.5 or higher
  2.  SQL Express LocalDB (v11.0) that is a new version of SQL Express created specifically for developers.
