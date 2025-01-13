using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.AuthorizationService;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class UpdateAuthorizationList
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.SimulationRoutes.UpdateAuthorizationList, async (UpdateAuthorizationListRequest request, HttpContext context, IMediator sender) =>
                {
                    UpdateAuthorizationListResponse response = await sender.Send(new UpdateAuthorizationListCommand(request));
                    return Results.Ok(response);
                })
                    .Produces<UpdateAuthorizationResponse>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status400BadRequest)
                    .Produces(StatusCodes.Status404NotFound)
                    .WithOpenApi(operation => new(operation)
                    {
                        OperationId = "UpdateAuthorizationList",
                        Summary = "Update authorization list",
                        Description = "Update authorization list"
                    })
                    .WithName("UpdateAuthorizationList")
                    .WithTags("Simulation")
                    .RequireAuthorization();
            }
        }

        public record UpdateAuthorizationListCommand(UpdateAuthorizationListRequest request) : IRequest<UpdateAuthorizationListResponse>
        {
            public UpdateAuthorizationListRequest Request { get; set; } = request;
        }

        public class Handler(IEuAuthorizationService authorizationService) : IRequestHandler<UpdateAuthorizationListCommand, UpdateAuthorizationListResponse>
        {
            private readonly IEuAuthorizationService _authorizationService = authorizationService;
            public async Task<UpdateAuthorizationListResponse> Handle(UpdateAuthorizationListCommand request, CancellationToken cancellationToken)
            {
                return await _authorizationService.UpdateAuthorizationList(request.Request);
            }
        }
    }
}
