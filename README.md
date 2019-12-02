# Chat System

This is a chat application written in C#. In the application you can create an account, add friends and chat with them.

The application is divide in three main sections:

## Front End

This is the client with which the users communicate. It's sends and receives user data from an API.

The front end is a ASP.NET MVC application that uses the following tools:

* [Bulma](https://bulma.io/) for the styles.
* [jQuery](https://jquery.com/) for some DOM manipulation.
* [Json.NET](https://www.newtonsoft.com/json) as JSON parser.
* [Microsoft.AspNet.WebApi.Client](https://www.nuget.org/packages/Microsoft.AspNet.WebApi.Client/) for the content negotation with the API.

## Back End

The back end is .NET Core Web API that uses the following tools:

* [AutoMapper](https://automapper.org/) for mapping different objects.
* [MongoDB Driver](https://automapper.org/) for interaction with MongoDB.
* [System.IdentityModel.Tokens.Jwt](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/) for token generation.

The API is constructed using the [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design#the-repository-pattern) that is used to manage data from databases and the [Request-Response Pattern](https://medium.com/@pulkitswarup/microservices-asynchronous-request-response-pattern-6d00ab78abb6) for the message exchange internally.

## External Process

This is a shared project that is used to encrypt and compress data using the [LZW](https://en.wikipedia.org/wiki/Lempel%E2%80%93Ziv%E2%80%93Welch) and [S-DES](https://en.wikipedia.org/wiki/Data_Encryption_Standard#Simplified_DES) algorithms.
