using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.UserAccess;
using CACIB.CREW.Api.Features.UserAccess.Model.DTO;
using CACIB.CREW.Api.Features.UserAccess.Model.Response;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.UserAccess.Handlers;

public class GetAllRoleMasters
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.UserAccessRoutes.GetAllRoleMasters, async (IMediator sender) =>
            {
                GetAllRoleMastersResponse response = new()
                {
                    Data = (await sender.Send(new Query())).ToList()
                };
                return Results.Ok(response);
            })
            .Produces<List<RoleMasterDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetAllRoleMasters",
                Summary = "Get all Role Master referential data Endpoint",
                Description = "Get all Role Master referential data Endpoint"
            })
            .WithName("GetAllRoleMasters")
            .WithTags("UserAccess");
        }
    }

    public record Query : IRequest<IEnumerable<RoleMasterDto>>
    {
        public Query()
        {
        }
    }

    public class Handler(IUserAccessService srv) : IRequestHandler<Query, IEnumerable<RoleMasterDto>>
    {
        private readonly IUserAccessService _srv = srv;

        public async Task<IEnumerable<RoleMasterDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            IEnumerable<RoleMasterDto> response = await _srv.GetRoleMasterReferential();

            return response;
        }
    }
}