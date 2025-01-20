using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Features.Unicorn.HttpClient;
using CACIB.CREW.Api.Features.Unicorn.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CACIB.CREW.Api.Features.Unicorn.Handler
{
    public static class SearchKyc
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.CrewUnicornRoutes.SearchKyc, ([FromBody] KycSearchRequest req, IMediator mediator)
                => mediator.Send(new Request(req)));
            }
        }
        public record Request : IRequest<KycSearchResponse>
        {
            public KycSearchRequest KycRequest { get; init; }
            public Request(KycSearchRequest req)
            {
                KycRequest = req;
            }
        }

        public class Handlerr(IUnicornHttpClient httpClient) : IRequestHandler<Request, KycSearchResponse>
        {
            readonly IUnicornHttpClient _httpClient = httpClient;

            public async Task<KycSearchResponse> Handle(Request request, CancellationToken cancellationToken)
            => await _httpClient.SearchKyc(request.KycRequest);

        }
    }
}
