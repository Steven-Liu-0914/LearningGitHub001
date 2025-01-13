using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers;

public class DuplicatePiSimulation
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(ApiRouteConstants.SimulationRoutes.DuplicatePiSimulation, async (DuplicateSimulationRequest request, HttpContext context, IMediator sender) =>
            {
                BaseResponse response = await sender.Send(new DuplicatePiSimulationCommand(request));
                return Results.Ok(response);
            })
                .Produces<BaseResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "DuplicateSimulation",
                    Summary = "Duplicate PI simulation endpoint",
                    Description = "Duplicate PI simulation"
                })
                .WithName("DuplicateSimulation")
                .WithTags("Simulation");
        }
    }

    public record DuplicatePiSimulationCommand : IRequest<BaseResponse>
    {
        public DuplicateSimulationRequest Request { get; set; }
        public DuplicatePiSimulationCommand(DuplicateSimulationRequest request)
        {
            Request = request;
        }
    }

    public class DuplicatePiSimulationHandler(ISimulationPiService simulationPiService) : IRequestHandler<DuplicatePiSimulationCommand, BaseResponse>
    {
        private readonly ISimulationPiService _simulationPiService = simulationPiService;
        public async Task<BaseResponse> Handle(DuplicatePiSimulationCommand command, CancellationToken cancellationToken)
        {
            return await _simulationPiService.DuplicatePiSimulation(command.Request);
        }
    }
}
