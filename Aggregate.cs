using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Features.Unicorn.HttpClient;
using CACIB.CREW.Api.Features.Unicorn.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CACIB.CREW.Api.Features.Unicorn.Handler;
public static class Aggregate
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(ApiRouteConstants.CrewUnicornRoutes.Aggregate, ([FromBody] AggregateRequest req, IMediator mediator)
            => mediator.Send(new Request(req)));
        }
    }
    public record Request : IRequest<AggregateResponse>
    {
        public AggregateRequest Aggregaterequest { get; init; }
        public Request(AggregateRequest req)
        {
            Aggregaterequest = req;
        }
    }

    public class Handler(IUnicornHttpClient httpClient) : IRequestHandler<Request, AggregateResponse>
    {
        readonly IUnicornHttpClient _httpClient = httpClient;
        public async Task<AggregateResponse> Handle(Request request, CancellationToken cancellationToken)
        => await _httpClient.Aggregate(request.Aggregaterequest);

    }
}