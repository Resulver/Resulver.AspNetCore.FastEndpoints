using Resulver.AspNetCore.Core.Abstraction;
using Resulver.AspNetCore.Core.ErrorHandling;

namespace Resulver.AspNetCore.FastEndpoints;

public abstract class ErrorProfile : ErrorProfile<FailureResponse>
{
    protected override ErrorResponseHandler<FailureResponse> AddError<TError>()
    {
        var errorResponse = new ErrorResponseHandler(typeof(TError));

        ErrorResponses.Add(errorResponse);

        return errorResponse;
    }
}