using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service;
using CACIB.CREW.Api.Features.Referential.Model;
using CACIB.CREW.Api.Features.StringResource.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Referential.Handler;

public static class GetStringResource
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.StringResourceRoutes.GetByKey, (string type, string key, string cultureCode, IMediator sender) =>
            sender.Send(new Query(type,key,cultureCode)));
        }
    }
    public record Query : IRequest<GetStringResourceByKeyResponse>
    {
        public string Type { get; init; }
        public string Key { get; init; }
        public string CultureCode { get; init; }

        public Query(string type, string key, string cultureCode)
        {
            Type = type;
            Key=key;
            CultureCode = cultureCode;
        }
    }

    public class Handler(IStringResourceService srv) : IRequestHandler<Query, GetStringResourceByKeyResponse>
    {
        readonly IStringResourceService _srv = srv;

        public async Task<GetStringResourceByKeyResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _srv.GetStringResourceByKey(request.Type,request.Key,request.CultureCode);
        }
    }
}
