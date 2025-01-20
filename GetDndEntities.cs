using CACIB.CREW.Api.Core.Cache;
using CACIB.CREW.Api.Core.Common;
using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Features.Unicorn.HttpClient;
using CACIB.CREW.Api.Features.Unicorn.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Unicorn.Handler;
public static class GetDndEntities
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.CrewUnicornRoutes.GetDndEntities,
            (IMediator mediator) => mediator.Send(new Query()));
        }
    }
    public record Query : IRequest<DndEntitiesResponse>
    {
        public Query()
        {
        }
    }

    public class Handler(IUnicornHttpClient httpClient,
                IUnicornDistributedCache distributedCache) : IRequestHandler<Query, DndEntitiesResponse>
    {
        readonly IUnicornHttpClient _httpClient = httpClient;
        readonly IUnicornDistributedCache _distributedCache = distributedCache;

        public async Task<DndEntitiesResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var data = await _distributedCache.GetAsync<DndEntitiesResponse>(CacheKey.DndEntities);
            if (data == null)
            {
                data = await _httpClient.GetDndEntities();
                await _distributedCache.SetAsync(CacheKey.DndEntities, data);
            }
            return data;
        }

    }
}