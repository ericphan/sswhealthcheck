sswhealthcheck
==============

SSW HealthCheck

Websites can be complicated, and a very small mistake can take the whole site down. There are two different kind of errors, coding errors and deployment errors; coding errors should be picked up by compiling and debugging, while deployment errors should be picked up by HealthCheck page.

Prerequisites
==============

1. Visual Studio 2013 (for older versions of Visual Studio, please install NuGet Package installer from http://nuget.codeplex.com/releases)
2. Your existing ASP.NET MVC 5 Application

Integration Steps
==============

Go to your Nuget Package Manager Console and type

Install-Package SSW.HealthCheck.Mvc5

Note: Alternatively you can use Tools | Library Package Manager | Manage NuGet Packages for Solution and follow the prompts.

Your project must have SignalR. Go to Startup.cs file, and make sure SignalR is mapped. If it is not, then map it.

Integration is completed now. You can run the application by hitting F5 and navigating to /HealthCheck page

