using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.SimulationAccess;
using CACIB.CREW.Api.Features.SimulationAccess.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.SimulationAccess.Handlers;

public class UpdateSimulationAccess
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(ApiRouteConstants.SimulationAccessRoutes.SimulationAccess, async (UpdateSimulationAccessRequest request, IMediator sender) =>
            {
                UpdateSimulationAccessResponse response = await sender.Send(new Query(request.Items, request.SimulationId));
                return Results.Ok(response);
            })
            .Produces<UpdateSimulationAccessResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "UpdateSimulationAccess",
                Summary = "Update list of users with their access",
                Description = "Update users list with their access rights for given credit file id with it's utcode, firsname, lastname and rights"
            })
            .WithName("UpdateSimulationAccess")
            .WithTags("SimulationAccess");
        }
    }

    public record Query(List<UpdateSimulationAccessDto> Simulations, long SimulationId) : IRequest<UpdateSimulationAccessResponse>
    {
    }

    public class Handler(ISimulationAccessService service) : IRequestHandler<Query, UpdateSimulationAccessResponse>
    {
        public async Task<UpdateSimulationAccessResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            return await service.UpdateSimulationAccesses(request.SimulationId, request.Simulations);
        }
    }
}
