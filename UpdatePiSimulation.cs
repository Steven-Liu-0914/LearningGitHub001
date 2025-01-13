using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers;

public class UpdatePiSimulation
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPut(ApiRouteConstants.SimulationPiRoutes.UpdatePiSimulation, async (/*FromRoute*/long id, UpdatePiSimulationRequest request, HttpContext context, IMediator sender) =>
            {
                UpdatePiSimulationResponse response = await sender.Send(new Command(id, request));
                return Results.Ok(response);
            })
                .Produces<UpdatePiSimulationResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "UpdatePiSimulation",
                    Summary = "Update an existing credit file",
                    Description = "Update an existing credit file for simulation pi"
                })
                .WithName("Update Pi Simulation")
                .WithTags("Simulation")
                .RequireAuthorization();
        }
    }

    public record Command(long id, UpdatePiSimulationRequest request) : IRequest<UpdatePiSimulationResponse>
    {
        public long Id { get; init; } = id;
        public string Name { get; init; } = request.Name;
        public string? Description { get; init; } = request.Description;
        public decimal Amount { get; init; } = request.Amount;
        public string Currency { get; init; } = request.Currency;
        public decimal ExchangeRate { get; init; } = request.ExchangeRate;
    }

    public class Handler(ISimulationPiService simulationPiService) : IRequestHandler<Command, UpdatePiSimulationResponse>
    {
        public async Task<UpdatePiSimulationResponse> Handle(Command command, CancellationToken cancellationToken)
        {
            return await simulationPiService.UpdateSimulation(command);
        }
    }
}