namespace Resulver.AspNetCore.FastEndpoints;

public class ResultBaseEp
{
    public static class Req<TRequest> where TRequest : notnull
    {
        public abstract class NoRes : NoRequestResultBaseEndpoint<TRequest>;

        public abstract class Res<TResponse> : ResultBaseEndpoint<TRequest, TResponse>;
    }

    public static class NoReq
    {
        public abstract class NoRes : NoRequestResultBaseEndpoint;
        public abstract class Res<TResponse> : NoRequestResultBaseEndpoint<TResponse>;
    }
}