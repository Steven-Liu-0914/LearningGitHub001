using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Lgd;
using CACIB.CREW.Api.Features.Lgd.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Lgd.Simulation.Handlers
{
    public class GetLgdProjectSimulation
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.LgdRoutes.GetLgdProjectSimulation, async (long id, IMediator sender) =>
                {
                    LgdProjectSimulationResponse response = await sender.Send(new Query(id));
                    return Results.Ok(response);
                })
                .Produces<LgdProjectSimulationResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetLgdProjectSimulation",
                    Summary = "Get lgd simulation data In CREW",
                    Description = "Get lgd simulation data In CREW"
                })
                .WithName("Lgd Simulation")
                .WithTags("Lgd Simulation");
            }
        }

        public record Query(long Id) : IRequest<LgdProjectSimulationResponse>
        {
        }

        public class Handler(ILgdService lgdSimulationService) : IRequestHandler<Query, LgdProjectSimulationResponse>
        {
            public async Task<LgdProjectSimulationResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                return await lgdSimulationService.GetLgdProjectSimulation(request.Id);
            }
        }
    }
}
