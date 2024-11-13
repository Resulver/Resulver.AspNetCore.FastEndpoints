using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Resulver.AspNetCore.FastEndpoints;

public class FailureResponse : ValidationFailure
{
    public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
}