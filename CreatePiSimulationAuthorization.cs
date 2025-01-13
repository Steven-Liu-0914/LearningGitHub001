using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.AuthorizationService;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class CreatePiSimulationAuthorization
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.SimulationRoutes.CreateAuthorization, async (SaveAuthorizationRequest request, HttpContext context, IMediator sender) =>
                {
                    SaveAuthorizationResponse response = await sender.Send(new CreatePiSimulationAuthorizationCommand(request));
                    return Results.Ok(response);
                })
                    .Produces<SaveAuthorizationResponse>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status400BadRequest)
                    .Produces(StatusCodes.Status404NotFound)
                    .WithOpenApi(operation => new(operation)
                    {
                        OperationId = "CreatePiSimulationAuthorization",
                        Summary = "Create authorization",
                        Description = "Create authorization"
                    })
                    .WithName("CreatePiSimulationAuthorization")
                    .WithTags("Simulation")
                    .RequireAuthorization();
            }
        }

        public record CreatePiSimulationAuthorizationCommand : IRequest<SaveAuthorizationResponse>
        {
            public SaveAuthorizationRequest Request { get; set; }
            public CreatePiSimulationAuthorizationCommand(SaveAuthorizationRequest request)
            {
                Request = request;
            }
        }

        public class Handler(IEuAuthorizationService authorizationService) : IRequestHandler<CreatePiSimulationAuthorizationCommand, SaveAuthorizationResponse>
        {
            private readonly IEuAuthorizationService _authorizationService = authorizationService;
            public async Task<SaveAuthorizationResponse> Handle(CreatePiSimulationAuthorizationCommand request, CancellationToken cancellationToken)
            {
                return await _authorizationService.SaveAuthorizationForPiSimulation(request.Request);
            }
        }
    }
}