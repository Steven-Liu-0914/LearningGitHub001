using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Features.Unicorn.HttpClient;
using CACIB.CREW.Api.Features.Unicorn.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CACIB.CREW.Api.Features.Unicorn.Handler;

public static class GetGroupHierarchy
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(ApiRouteConstants.CrewUnicornRoutes.GetGroupHierarchy, ([FromBody] GroupHierarchyRequest req, IMediator mediator)
            => mediator.Send(new Request(req)));
        }
    }
    public record Request : IRequest<GroupHierarchyResponse>
    {
        public GroupHierarchyRequest Hierarchyrequest { get; init; }
        public Request(GroupHierarchyRequest req)
        {
            Hierarchyrequest = req;
        }
    }

    public class Handlerr(IUnicornHttpClient httpClient) : IRequestHandler<Request, GroupHierarchyResponse>
    {
        readonly IUnicornHttpClient _httpClient = httpClient;

        public async Task<GroupHierarchyResponse> Handle(Request request, CancellationToken cancellationToken)
        => await _httpClient.GetGroupHierarchy(request.Hierarchyrequest);

    }
}