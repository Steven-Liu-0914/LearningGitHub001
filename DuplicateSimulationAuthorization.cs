using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.AuthorizationService;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class DuplicateSimulationAuthorization
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.SimulationRoutes.DuplicateAuthorization, async (DuplicateSimulationAuthorizationRequest request, HttpContext context, IMediator sender) =>
                {
                    DuplicateSimulationAuthorizationResponse response = await sender.Send(new DuplicateSimulationAuthorizationCommand(request));
                    return Results.Ok(response);
                })
                    .Produces<DuplicateSimulationAuthorizationResponse>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status400BadRequest)
                    .Produces(StatusCodes.Status404NotFound)
                    .WithOpenApi(operation => new(operation)
                    {
                        OperationId = "DuplicateSimulationAuthorization",
                        Summary = "Duplicate authorization",
                        Description = "Duplicate authorization for simulation"
                    })
                    .WithName("DuplicatePiSimulationAuthorization")
                    .WithTags("Simulation")
                    .RequireAuthorization();
            }
        }

        public record DuplicateSimulationAuthorizationCommand : IRequest<DuplicateSimulationAuthorizationResponse>
        {
            public DuplicateSimulationAuthorizationRequest Request { get; set; }
            public DuplicateSimulationAuthorizationCommand(DuplicateSimulationAuthorizationRequest request)
            {
                Request = request;
            }
        }

        public class Handler(IEuAuthorizationService authorizationService) : IRequestHandler<DuplicateSimulationAuthorizationCommand, DuplicateSimulationAuthorizationResponse>
        {
            private readonly IEuAuthorizationService _authorizationService = authorizationService;
            public async Task<DuplicateSimulationAuthorizationResponse> Handle(DuplicateSimulationAuthorizationCommand request, CancellationToken cancellationToken)
            {
                return await _authorizationService.DuplicateAuthorizationForSimulation(request.Request);
            }
        }
    }
}