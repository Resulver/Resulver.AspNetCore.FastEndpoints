[![NuGet Package](https://img.shields.io/nuget/v/Resulver.AspNetCore.FastEndpoints)](https://www.nuget.org/packages/Resulver.AspNetCore.FastEndpoints/)

### Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [Error Handling](#error-handling)
    - [Adding Resulver to Your Application](#1-adding-resulver-to-your-application)
    - [Creating Error Profiles](#2-creating-error-profiles)
- [Using in Endpoints](#using-in-endpoints)
    - [Method 1: Inheritance from the ResultBaseEndpoint Class](#method-1-inheritance-from-the-resultbaseendpoint-class)
    - [Method 2: Inject IErrorResponseGenerator](#method-2-inject-ierrorresponsegenerator)

---

### Overview

`Resulver.AspNetCore.FastEndpoints` is an extension of the `Resulver` library that integrates seamlessly with FastEndpoints. It simplifies structured error handling and result management, allowing you to focus on your business logic while the library takes care of consistent responses and error profiles.

---

### Installation

To install the `Resulver.AspNetCore.FastEndpoints` package, use the following command:

```bash
dotnet add package Resulver.AspNetCore.FastEndpoints
```

Ensure you have the required .NET SDK installed.

---

### Error Handling

#### 1. Adding Resulver to Your Application

To enable `Resulver` in your application, add the following code to the `Program.cs` file:

```csharp
builder.Services.AddResulver(Assembly.GetExecutingAssembly());
```

This registers all error profiles and ensures they are used when generating responses.

#### 2. Creating Error Profiles

Error profiles define how specific error types should be translated into HTTP responses. For example:

1. Define a custom error class:

   ```csharp
   public class ValidationError(string title, string message) : ResultError(message, title: title);
   ```

2. Create an error profile for the custom error:

   ```csharp
   public class ValidationErrorProfile : ErrorProfile
   {
       public override void Configure()
       {
           AddError<ValidationError>().WithStatusCode(400);
       }
   }
   ```

With this profile, any `ValidationError` returned in a result will automatically generate a `400 Bad Request` response.

---

### Using in Endpoints

#### Method 1: Inheritance from the ResultBaseEndpoint Class

This approach simplifies result handling by using the `ResultBaseEndpoint` base class:

```csharp
public class MyEndpoints : ResultBaseEndpoint<string, string>
{
    public override void Configure()
    {
        Post("my-endpoint");
        AllowAnonymous();
    }

    public override Task HandleAsync(string req, CancellationToken ct)
    {
        // Logic
        var result = new Result<string>("this is result message");

        // Return a response generated from the result
        return SendFromResultAsync(result, 200, ct);
    }
}
```

**Note:** If the `Result` contains errors, the response will automatically be generated based on the error profile defined for those errors.

#### Method 2: Inject `IErrorResponseGenerator` into Your Endpoint

For greater flexibility, you can inject `IErrorResponseGenerator` to manually handle errors:

```csharp
public class MyEndpoints : Ep.Req<string>.Res<string>
{
    public required IErrorResponseGenerator<FailureResponse> ErrorResponseGenerator { get; init; }

    public override void Configure()
    {
        Post("my-endpoint");
        AllowAnonymous();
    }

    public override Task HandleAsync(string req, CancellationToken ct)
    {
        // Logic
        var result = new Result<string>("this is result message");

        if (result.IsFailure)
        {
            // Generate a failure response for the first error
            var failureResponse = ErrorResponseGenerator.MakeResponse(result.Errors[0]);
            AddError(failureResponse);

            // Send the error response
            return SendErrorsAsync(failureResponse.StatusCode, ct);
        }

        return SendOkAsync(result.Message, ct);
    }
}
```

