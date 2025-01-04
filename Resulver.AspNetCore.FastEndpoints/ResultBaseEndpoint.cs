using FastEndpoints;
using Resulver.AspNetCore.Core.Abstraction;
using Resulver.AspNetCore.Core.Response;

namespace Resulver.AspNetCore.FastEndpoints;

public abstract class ResultBaseEndpoint<TRequest> : Ep.Req<TRequest>.Res<ResponseTemplate>
    where TRequest : notnull
{
    public required IErrorResponseGenerator<FailureResponse> ErrorResponseGenerator { get; init; }

    protected Task SendResultErrorAsync(ResultError error, CancellationToken ct)
    {
        var failureResponse = ErrorResponseGenerator.MakeResponse(error);

        AddError(failureResponse);

        return SendErrorsAsync(failureResponse.StatusCode, ct);
    }

    protected Task SendFromResultAsync(Result result, int statusCode, CancellationToken ct)
    {
        return result.IsSuccess
            ? SendAsync(result.ToResponseTemplate(), 200, ct)
            : SendResultErrorAsync(result.Errors[0], ct);
    }
}

public abstract class
    ResultBaseEndpoint<TRequest, TResponseContent> : Ep.Req<TRequest>.Res<ResponseTemplate<TResponseContent>>
    where TRequest : notnull
{
    public required IErrorResponseGenerator<FailureResponse> ErrorResponseGenerator { get; init; }

    protected Task SendResultErrorAsync(ResultError error, CancellationToken ct)
    {
        var failureResponse = ErrorResponseGenerator.MakeResponse(error);

        AddError(failureResponse);

        return SendErrorsAsync(failureResponse.StatusCode, ct);
    }

    protected Task SendFromResultAsync(Result<TResponseContent> result, int statusCode, CancellationToken ct)
    {
        return result.IsSuccess
            ? SendAsync(result.ToResponseTemplate(), 200, ct)
            : SendResultErrorAsync(result.Errors[0], ct);
    }
}