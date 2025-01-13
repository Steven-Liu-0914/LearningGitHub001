using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.UserAccess;
using CACIB.CREW.Api.Features.SearchRcpUser.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.SearchRcpUser.Handlers;

public class GetRcpUsers
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.SearchRcpUserRoutes.SearchRcpUsers, async (string key, IMediator sender) =>
            {
                SearchRcpUserResponse response = await sender.Send(new Query(key));
                return Results.Ok(response);
            })
            .Produces<SearchRcpUserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetRcpUsers",
                Summary = "Get user list for given search text",
                Description = "Get list of users with it's utcode and fullname"
            })
            .WithName("GetRcpUsers")
            .WithTags("Search Rcp");
        }
    }

    public record Query : IRequest<SearchRcpUserResponse>
    {
        public string SearchText { get; init; }

        public Query(string searchText)
        {
            SearchText = searchText;
        }
    }

    public class Handler(ISearchRcpUsersService srv) : IRequestHandler<Query, SearchRcpUserResponse>
    {
        private readonly ISearchRcpUsersService _srv = srv;

        public async Task<SearchRcpUserResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _srv.Search(request.SearchText);
        }
    }
}
