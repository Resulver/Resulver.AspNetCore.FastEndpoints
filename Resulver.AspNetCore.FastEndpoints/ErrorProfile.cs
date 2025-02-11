using Microsoft.AspNetCore.Http;
using Resulver.AspNetCore.Core.Abstraction;
using Resulver.AspNetCore.Core.ErrorHandling;

namespace Resulver.AspNetCore.FastEndpoints;

public abstract class ErrorProfile : ErrorProfile<IResult>
{
    protected override ErrorResponseHandler<IResult> AddError<TError>()
    {
        var errorResponse = new ErrorResponseHandler(typeof(TError));

        ErrorResponses.Add(errorResponse);

        return errorResponse;
    }
}