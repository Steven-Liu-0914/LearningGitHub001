using CACIB.CREW.Api.Core.Service.AuthorizationService;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static CACIB.CREW.Api.Core.Route.ApiRouteConstants;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class GetAuthorization : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(SimulationRoutes.GetAuthorization, async ([FromRoute]int id, IMediator sender) =>
            {
                var result = await sender.Send(new GetAuthorizationQuery(id));
                return Results.Ok(result);
            })
            .Produces<GetAuthorizationResponse>(StatusCodes.Status200OK)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetAuthorization",
                Summary = "Get the authorization",
                Description = "This is an API used to get authorization based on authId in CREW"
            })
           .WithName("GetAuthorization")
           .WithTags("Simulation");
        }
    }

    public record GetAuthorizationQuery(int AuthorizationId) : IRequest<GetAuthorizationResponse> { }

    public class GetAuthorizationHandler(IEuAuthorizationService euAuthorizationService) : IRequestHandler<GetAuthorizationQuery, GetAuthorizationResponse>
    {
        public async Task<GetAuthorizationResponse> Handle(GetAuthorizationQuery request, CancellationToken cancellationToken)
        {
            return await euAuthorizationService.GetAuthorization(request.AuthorizationId);
        }
    }
}
