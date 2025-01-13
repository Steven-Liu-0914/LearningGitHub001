using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Lgd;
using CACIB.CREW.Api.Features.Lgd.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class DuplicateLGDSimulation
    {
        public class EndPoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.SimulationRoutes.DuplicateLGDSimulation, async (DuplicateLgdSimulationCommand request, IMediator sender) =>
                {
                    LgdProjectSimulationResponse response = await sender.Send(request);
                    return Results.Ok(response);
                }).Produces<LgdProjectSimulationResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "CopyLgdProjectSimulation",
                    Summary = "Duplicate lgd simulation in CREW",
                    Description = "Duplicate lgd simulation in CREW"
                })
                .WithName("Copy Lgd Simulation")
                .WithTags("Simulation");
            }
        }

        public record DuplicateLgdSimulationCommand(long Id, string Name) : IRequest<LgdProjectSimulationResponse>
        {
            public long Id { get; set; } = Id;
            public string Name { get; set; } = Name;

        }

        public class Handler(ILgdService lgdSimulationService) : IRequestHandler<DuplicateLgdSimulationCommand, LgdProjectSimulationResponse>
        {
            public async Task<LgdProjectSimulationResponse> Handle(DuplicateLgdSimulationCommand request, CancellationToken cancellationToken)
            {
                return await lgdSimulationService.DuplicateLgdSimulation(request.Id, request.Name);
            }
        }
    }
}
