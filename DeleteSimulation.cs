using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class DeleteSimulation
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapDelete(ApiRouteConstants.SimulationRoutes.DeleteSimulation, async (long id, IMediator sender) =>
                {
                    BaseResponse response = await sender.Send(new Query(id));
                    return Results.Ok(response);
                })
                .Produces<bool>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "DeleteSimulation",
                    Summary = "Delete selected simulation from search results",
                    Description = "This is an API Used to delete simulation in CREW"
                })
                .WithName("DeleteSimulation")
                .WithTags("Simulation");
            }
        }

        public record Query(long Id) : IRequest<BaseResponse>
        {
        }

        public class Handler(ISimulationService simulationService) : IRequestHandler<Query, BaseResponse>
        {
            public async Task<BaseResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                return await simulationService.DeleteSimulation(request.Id);
            }
        }
    }
}
