using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.AuthorizationService;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class UpdatePiSimulationAuthorization
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPut(ApiRouteConstants.SimulationRoutes.UpdateAuthorization, async (int id, UpdateAuthorizationRequest request, HttpContext context, IMediator sender) =>
                {
                    UpdateAuthorizationResponse response = await sender.Send(new UpdatePiSimulationAuthorizationCommand(id, request));
                    return Results.Ok(response);
                })
                    .Produces<UpdateAuthorizationResponse>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status400BadRequest)
                    .Produces(StatusCodes.Status404NotFound)
                    .WithOpenApi(operation => new(operation)
                    {
                        OperationId = "UpdatePiSimulationAuthorization",
                        Summary = "Update authorization",
                        Description = "Update authorization"
                    })
                    .WithName("UpdatePiSimulationAuthorization")
                    .WithTags("Simulation")
                    .RequireAuthorization();
            }
        }

        public record UpdatePiSimulationAuthorizationCommand(int id, UpdateAuthorizationRequest request) : IRequest<UpdateAuthorizationResponse>
        {
            public int Id { get; set; } = id;
            public UpdateAuthorizationRequest Request { get; set; } = request;
        }

        public class Handler(IEuAuthorizationService authorizationService) : IRequestHandler<UpdatePiSimulationAuthorizationCommand, UpdateAuthorizationResponse>
        {
            private readonly IEuAuthorizationService _authorizationService = authorizationService;
            public async Task<UpdateAuthorizationResponse> Handle(UpdatePiSimulationAuthorizationCommand request, CancellationToken cancellationToken)
            {
                return await _authorizationService.UpdateAuthorizationForPiSimulation(request.id, request.Request);
            }
        }
    }
}
