using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class UpdatePiSimulationClient
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPut(ApiRouteConstants.SimulationPiRoutes.UpdatePiSimulationClient, async (PiSimulationClientRequest request, HttpContext context, IMediator sender) =>
                {
                    UpdatePiSimulationClientResponse response = await sender.Send(new UpdatePiSimulationClientCommand(request));
                    return Results.Ok(response);
                })
                    .Produces<UpdatePiSimulationClientResponse>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status400BadRequest)
                    .Produces(StatusCodes.Status404NotFound)
                    .WithOpenApi(operation => new(operation)
                    {
                        OperationId = "UpdatePiSimulationClient",
                        Summary = "Update PI simulation client",
                        Description = "Update client for a PI simulation"
                    })
                    .WithName("UpdatePiSimulationClient")
                    .WithTags("Simulation")
                    .RequireAuthorization();
            }
        }

        public record UpdatePiSimulationClientCommand : IRequest<UpdatePiSimulationClientResponse>
        {
            public PiSimulationClientRequest Request { get; set; }
            public UpdatePiSimulationClientCommand(PiSimulationClientRequest request)
            {
                Request = request;
            }
        }

        public class UpdatePiSimulationClientHandler(ISimulationPiService simulationPiService) : IRequestHandler<UpdatePiSimulationClientCommand, UpdatePiSimulationClientResponse>
        {
            private readonly ISimulationPiService _simulationPiService = simulationPiService;
            public async Task<UpdatePiSimulationClientResponse> Handle(UpdatePiSimulationClientCommand request, CancellationToken cancellationToken)
            {
                return await _simulationPiService.UpdateSimulationClient(request.Request);
            }
        }
    }
}
