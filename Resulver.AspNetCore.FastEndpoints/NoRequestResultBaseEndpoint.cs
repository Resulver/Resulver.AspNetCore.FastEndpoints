using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Resulver.AspNetCore.Core.Abstraction;
using Resulver.AspNetCore.Core.Response;

namespace Resulver.AspNetCore.FastEndpoints;

public abstract class NoRequestResultBaseEndpoint : Ep.NoReq.Res<ResponseTemplate>
{
    public required IErrorResponseGenerator<IResult> ErrorResponseGenerator { get; init; }

    protected Task SendResultErrorAsync(ResultError error, CancellationToken ct)
    {
        var failureResponse = ErrorResponseGenerator.MakeResponse(error);

        return SendResultAsync(failureResponse);
    }

    protected Task SendFromResultAsync(Result result, int statusCode, CancellationToken ct)
    {
        return result.IsSuccess
            ? SendAsync(result.ToResponseTemplate(), 200, ct)
            : SendResultErrorAsync(result.Errors[0], ct);
    }
}

public abstract class NoRequestResultBaseEndpoint<TResponseContent> : Ep.NoReq.Res<ResponseTemplate<TResponseContent>>
{
    public required IErrorResponseGenerator<IResult> ErrorResponseGenerator { get; init; }

    protected Task SendResultErrorAsync(ResultError error, CancellationToken ct)
    {
        var failureResponse = ErrorResponseGenerator.MakeResponse(error);

        return SendResultAsync(failureResponse);
    }

    protected Task SendFromResultAsync(Result<TResponseContent> result, int statusCode, CancellationToken ct)
    {
        return result.IsSuccess
            ? SendAsync(result.ToResponseTemplate(), 200, ct)
            : SendResultErrorAsync(result.Errors[0], ct);
    }
}