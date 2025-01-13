using Azure;
using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers;

public class GetClientHierarchy
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.SimulationRoutes.GetClientHierarchy, async (/*FromRoute*/ long id, HttpContext context, IMediator sender) =>
            {
                GetClientHierarchyResponse response = await sender.Send(new Query(id));
                return Results.Ok(response);
            })
                .Produces<GetClientHierarchyResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetClientHierarchy",
                    Summary =  "Get client hierarchy",
                    Description = "Get client hierarchy by device In CREW"
                })
                .WithName("Get Client Hierarchy")
                .WithTags("Simulation")
                .RequireAuthorization();
        }
    }

    public record Query(long Id) : IRequest<GetClientHierarchyResponse>;

    public class Handler(ISimulationService simulationService) : IRequestHandler<Query, GetClientHierarchyResponse>
    {
        public async Task<GetClientHierarchyResponse> Handle(Query query, CancellationToken cancellationToken)
        {
            return await simulationService.GetClientHierarchy(query.Id);
        }
    }
}