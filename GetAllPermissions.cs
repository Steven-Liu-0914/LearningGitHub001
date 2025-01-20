using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.UserAccess;
using CACIB.CREW.Api.Features.UserAccess.Model.DTO;
using CACIB.CREW.Api.Features.UserAccess.Model.Response;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.UserAccess.Handlers;

public class GetAllPermissions
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.UserAccessRoutes.GetAllPermissions, async (IMediator sender) =>
            {
                GetAllPermissionsResponse response = new()
                {
                    Data = (await sender.Send(new Query())).ToList()
                };
                return Results.Ok(response);
            })
            .Produces<List<PermissionDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetAllPermissions",
                Summary = "Get all Permissions referential data Endpoint",
                Description = "Get all Permissions referential data Endpoint"
            })
            .WithName("GetAllPermissions")
            .WithTags("UserAccess");
        }
    }

    public record Query : IRequest<IEnumerable<PermissionDto>>
    {
        public Query()
        {
        }
    }

    public class Handler(IUserAccessService srv) : IRequestHandler<Query, IEnumerable<PermissionDto>>
    {
        private readonly IUserAccessService _srv = srv;

        public async Task<IEnumerable<PermissionDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            IEnumerable<PermissionDto> response = await _srv.GetPermissionsReferential();

            return response;
        }
    }
}