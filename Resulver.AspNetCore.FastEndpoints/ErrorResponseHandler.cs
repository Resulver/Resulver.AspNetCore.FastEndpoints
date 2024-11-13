using Resulver.AspNetCore.Core.ErrorHandling;

namespace Resulver.AspNetCore.FastEndpoints;

public class ErrorResponseHandler : ErrorResponseHandler<FailureResponse>
{
    public ErrorResponseHandler(Type errorType)
    {
        Handler = e => new FailureResponse
        {
            StatusCode = StatusCode, PropertyName = e.Title, ErrorMessage = e.Message
        };

        ErrorType = errorType;
    }

    public sealed override Func<ResultError, FailureResponse> Handler { get; protected set; }

    public override Type ErrorType { get; }
}