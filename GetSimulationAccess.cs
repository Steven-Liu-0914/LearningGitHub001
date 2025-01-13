using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.SimulationAccess;
using CACIB.CREW.Api.Features.SimulationAccess.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.SimulationAccess.Handlers;

public class GetSimulationAccess
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.SimulationAccessRoutes.SimulationAccess, async (long simulationId, IMediator sender) =>
            {
                GetSimulationAccessResponse response = await sender.Send(new Query(simulationId));
                return Results.Ok(response);
            })
            .Produces<GetSimulationAccessResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetSimulationAccess",
                Summary = "Get list users with their access",
                Description = "Get list users with their access for given credit file id with it's utcode, firsname, lastname and rights"
            })
            .WithName("GetSimulationAccess")
            .WithTags("SimulationAccess");
        }
    }

    public record Query(long CreditFileId) : IRequest<GetSimulationAccessResponse>
    {
    }

    public class Handler(ISimulationAccessService service) : IRequestHandler<Query, GetSimulationAccessResponse>
    {
        public async Task<GetSimulationAccessResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            return await service.GetSimulationAccesses(request.CreditFileId);
        }
    }
}
