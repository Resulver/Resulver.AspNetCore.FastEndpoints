using Microsoft.AspNetCore.Http;
using Resulver.AspNetCore.Core.ErrorHandling;

namespace Resulver.AspNetCore.FastEndpoints;

public class ErrorResponseHandler : ErrorResponseHandler<IResult>
{
    public ErrorResponseHandler(Type errorType)
    {
        Handler = e => Results.Json(
            statusCode: StatusCode,
            data: e);

        ErrorType = errorType;
    }

    public sealed override Func<ResultError, IResult> Handler { get; protected set; }

    public override Type ErrorType { get; }
}