using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class GetPiSimulation
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.SimulationPiRoutes.GetPiSimulation, async (long id, IMediator sender) =>
                {
                    GetPiSimulationResponse response = await sender.Send(new Query(id));
                    return Results.Ok(response);
                })
                .Produces<GetPiSimulationResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetPiProjectSimulation",
                    Summary = "Get Pi simulation data In CREW",
                    Description = "Get Pi simulation data In CREW"
                })
                .WithName("GetPiProjectSimulation")
                .WithTags("Simulation");
            }
        }

        public record Query(long Id) : IRequest<GetPiSimulationResponse>
        {
            public long Id { get; init; } = Id;
        }

        public class Handler(ISimulationPiService simulationPiService) : IRequestHandler<Query, GetPiSimulationResponse>
        {
            public async Task<GetPiSimulationResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                return await simulationPiService.GetPiSimulation(request.Id);
            }
        }
    }
}
