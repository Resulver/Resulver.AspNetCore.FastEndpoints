[![NuGet Package](https://img.shields.io/nuget/v/Resulver.AspNetCore.FastEndpoints)](https://www.nuget.org/packages/Resulver.AspNetCore.FastEndpoints/)



### Table of content
- [Installation](#Installation)
- [Error Handling](#Error-Handling)
- [Using in Endpoints](#Using-in-Endpoints)
## Installation
  ```bash
  dotnet add package Resulver.AspNetCore.FastEndpoints
  ```

## Error Handling

### 1. First, add the following code to the Program.cs file:

```csharp
builder.Services.AddResulver(Assembly.GetExecutingAssembly());
```

### 2. Create Error Profiles
In this example, we have created an error named ValidationError :
```csharp
public class ValidationError(string title, string message) : ResultError(message, title: title);
```
Now, to manage responses related to this error, we will create a profile for it.

```csharp
public class ValidationErrorProfile : ErrorProfile
{
    public override void Configure()
    {
        AddError<ValidationError>().WithStatusCode(400);
    }
}
```

## Using in Endpoints
### You can use the following methods to utilize IResultHandler in your Endpoints :
#### Method 1: Inheritance from the ResultBaseEndpoint class : 
```csharp
public class MyEndpoints : ResultBaseEndpoint<string,string>
{
    public override void Configure()
    {
        Post("my-endpoint");
        AllowAnonymous();
    }

    public override Task HandleAsync(string req, CancellationToken ct)
    {
        // logic
        //
        //
        
        var result = new Result<string>("this is result message");

        return SendFromResultAsync(result,200,ct);
    }
}
```
Note: In all cases, if the Result contains an error, a response will be generated based on the error profile created for that error.
#### Method 2: Inject IErrorResponseGenerator in to your Endpoint:
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
        // logic
        //
        //
        
        var result = new Result<string>("this is result message");


        if (result.IsFailure)
        {
            var failureResponse = ErrorResponseGenerator.MakeResponse(result.Errors[0]) ;
            AddError(failureResponse);
            
            return SendErrorsAsync(failureResponse.StatusCode,ct);
        }

        return SendOkAsync(result.Message,ct);
    }
}
```

