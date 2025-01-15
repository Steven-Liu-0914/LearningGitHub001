using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Lgd;
using CACIB.CREW.Api.Features.Apl.Model.Request;
using CACIB.CREW.Api.Features.Apl.Model.Response;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Apl.Handler;

public class GetAplCalculationResult
{
    public class Endpoint : IEndpoint
    {
        // We expose the endpoints for getting APL Results
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(ApiRouteConstants.LgdRoutes.GetAplCalculation, async (LgdProjectRequest request, IMediator sender) =>
            {
                LgdProjectResponse response = await sender.Send(new Query(request));
                return Results.Ok(response);
            })
            .Produces<LgdProjectResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetAplResult",
                Summary = "GetAplResult Endpoint",
                Description = "GetAplResult"
            })
            .WithName("GetAplResult")
            .WithTags("Lgd Simulation")
            .RequireAuthorization();
        }
    }

    public record Query(LgdProjectRequest Request) : IRequest<LgdProjectResponse>
    {
    }

    public class Handler(ILgdService lgdService) : IRequestHandler<Query, LgdProjectResponse>
    {
        public async Task<LgdProjectResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            return await lgdService.UpsertLgdResult(request.Request);
        }
    }
}
