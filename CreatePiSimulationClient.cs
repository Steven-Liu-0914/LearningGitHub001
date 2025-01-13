using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers;

public class CreatePiSimulationClient
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(ApiRouteConstants.SimulationPiRoutes.CreatePiSimulationClient, async (PiSimulationClientRequest request, HttpContext context, IMediator sender) =>
            {
                BaseResponse response = await sender.Send(new CreatePiSimulationClientCommand(request));
                return Results.Ok(response);
            })
                .Produces<BaseResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "CreatePiSimulationClient",
                    Summary = "Create PI simulation client endpoint",
                    Description = "Create client for a PI simulation"
                })
                .WithName("CreatePiSimulationClient")
                .WithTags("Simulation")
                .RequireAuthorization();
        }
    }
}

public record CreatePiSimulationClientCommand : IRequest<BaseResponse>
{
    public PiSimulationClientRequest Request { get; set; }
    public CreatePiSimulationClientCommand(PiSimulationClientRequest request)
    {
        Request = request;
    }
}

public class CreatePiSimulationClientHandler(ISimulationPiService simulationPiService) : IRequestHandler<CreatePiSimulationClientCommand, BaseResponse>
{
    private readonly ISimulationPiService _simulationPiService = simulationPiService;
    public async Task<BaseResponse> Handle(CreatePiSimulationClientCommand request, CancellationToken cancellationToken)
    {
        return await _simulationPiService.CreateSimulationClient(request.Request);
    }
}
