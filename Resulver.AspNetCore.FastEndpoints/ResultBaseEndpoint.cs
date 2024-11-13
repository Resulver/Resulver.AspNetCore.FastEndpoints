using FastEndpoints;
using Resulver.AspNetCore.Core.Abstraction;
using Resulver.AspNetCore.Core.Response;

namespace Resulver.AspNetCore.FastEndpoints;

public abstract class ResultBaseEndpoint<TRequest> : Ep.Req<TRequest>.Res<ResponseTemplate>
    where TRequest : notnull
{
    public required IErrorResponseGenerator<FailureResponse> ErrorResponseGenerator { get; init; }

    protected Task SendFromResultAsync(Result result, int statusCode, CancellationToken ct)
    {
        if (result.IsSuccess) return SendAsync(result.ToResponseTemplate(), 200, ct);
        
        var failureResponse = ErrorResponseGenerator.MakeResponse(result.Errors[0]);
        AddError(failureResponse);

        return SendErrorsAsync(failureResponse.StatusCode, ct);

    }
}

public abstract class ResultBaseEndpoint<TRequest,TResponseContent> : Ep
    .Req<TRequest>
    .Res<ResponseTemplate<TResponseContent>>
    where TRequest : notnull
{
    public required IErrorResponseGenerator<FailureResponse> ErrorResponseGenerator { get; init; }

    protected Task SendFromResultAsync(Result<TResponseContent> result, int statusCode, CancellationToken ct)
    {
        if (result.IsSuccess) return SendAsync(result.ToResponseTemplate(), 200, ct);
        
        var failureResponse = ErrorResponseGenerator.MakeResponse(result.Errors[0]);
        AddError(failureResponse);

        return SendErrorsAsync(failureResponse.StatusCode, ct);

    }
}