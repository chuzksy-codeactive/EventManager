# Event Management System
Event management is used as a solution for scheduling conference meetings and classrooms, coordinating events and room setup, assigning resources and services for an event

## Table of Content

-   [Motivation](https://github.com/chuzksy-codeactive/EventManager/#motivation)
-   [Features](https://github.com/chuzksy-codeactive/EventManager/#features)
-   [Installations](https://github.com/chuzksy-codeactive/EventManager/#installations)
-   [Instructions](https://github.com/chuzksy-codeactive/EventManager/#instructions)
-   [API Reference](https://github.com/chuzksy-codeactive/EventManager/#api-reference)
-   [Limitation](https://github.com/chuzksy-codeactive/EventManager/#limitation)
-   [Contribution](https://github.com/chuzksy-codeactive/EventManager/#contribution)
-   [Credits](https://github.com/chuzksy-codeactive/EventManager/#credits)
-   [Contact Information](https://github.com/chuzksy-codeactive/EventManager/#contact-information)
-   [License](https://github.com/chuzksy-codeactive/EventManager/#license)

## Motivation
This project is built just to acquire the basic skills on how to build an ASP.NET Core Web API applications. 
I have a similar project which is developed using Node.js and Express. Having learnt C# and EF Core, I decided to develop same project this time using ASP.NET Core Web API. 

## Features
### The following are the features built into the project
1. Basic CRUD operations for Users, Centers and Events
2. JWT authentication
3. Swagger Documentation
4. Pagination, Filtering, Sorting, Data shaping
5. Content Negotiation (application/json, application/xml, application/vnd.marvin.hateoas+json)
6. Support for HATEOAS
7. Pagination, Filtering, Sorting, Searching and Data Shaping
8. Caching
9. Validation using FluentValidator
10. Object Mapping using AutoMapper

## Installation
### Tools needed to be installed before running the project
1. Install latest version of .NET Core. Preferably v 3.1 SDK [here](https://dotnet.microsoft.com/download)
2. MSSQL server 2017 or above should also be installed. On Mac systems, I used docker and an image of mssql server 2017 container. Check [here](https://database.guide/how-to-install-sql-server-on-a-mac/) for more info
3. [Visual Studio](https://visualstudio.microsoft.com/downloads/) Code my preferably IDE of choice. But Visual Studio community edition will be fine.
  ***Note***, EF Core is a .NET Standard 2.1 library. So EF Core requires a .NET implementation that supports .NET standard 2.1 to run. 

## Instructions
1. Clone the repository
2. Change directory into projects directory `cd EventManager.API`
3. Create an empty database called `EventManager`
4. Change the password of the connection string found in the `appsettings.json` file
5. `dotnet ef` must be installed as a global or local tool. Type the following or click here for more info
* `dotnet tool install —global dotnet-ef`
6. Install dependencies by the following in the terminal `dotnet restore`
7. In the terminal type the following
  * `dotnet ef migrations add -o Data/Migrations Initial`
  * `dotnet ef database update` 
  * `dotnet build`
  * `dotnet run`
8. On the browser preferably [Brave](https://brave.com/sua484) type `https://localhost:5001/swagger/index.html` or `http://localhost:5000/swagger/index.html`. There you will find the Web API documentation in swagger

## API Reference
### The link below can be used as a reference to the API documenation. 
  * [![Run in Postman](https://run.pstmn.io/button.svg)](https://documenter.getpostman.com/view/5716924/SWECVEib?version=latest)
  * You can also run the project with `dotnet run` and then checkout the web api documentation in swagger with `https://localhost:5001/swagger/index.html` or `http://localhost:5000/swagger/index.html`

## Limitation

## Contribution
As said earlier, this project is built just to acquire the skills on how to build ASP.NET Core Web API applications. However, if you choose to contribute to this project
1. Fork this repository
2. Follow installations and how to use steps
3. Create your feature branch off the develop branch
4. Make changes and c commit your changes
5. Raise a pull request against develop branch

## Credits
Watching Kelvin Dockx's dotnet core tutorials on Pluralsight really inspired me to build this project. Links to his tutorials can be found below
1. [Building a RESTful API with ASP.NET Core 3](https://app.pluralsight.com/library/courses/asp-dot-net-core-3-restful-api-building/)
2. [Building an Async API with ASP.NET Core](https://app.pluralsight.com/library/courses/building-async-api-aspdotnet-core/)
3. The following are blog posts I read in other to understand some concepts like:
  * [Dependency Injection](https://medium.com/volosoft/asp-net-core-dependency-injection-best-practices-tips-tricks-c6e9c67f9d96)
  * [JWT authentication](https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api)

## Contact Information
For any questions and support, you can contact the author [Onuchukwu Chika](mailto:onuchukwu.chika@andela.com)

## License
Licensed under the [MIT License](https://github.com/chuzksy-codeactive/EventManager/blob/master/LICENSE) copyright(c) 2019 chuzksy