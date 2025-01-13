using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class GetPiSimulationClient
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.SimulationPiRoutes.SearchPiSimulationClients, async ([AsParameters] GetPiSimulationClientRequest request, IMediator sender) =>
                {
                    GetPiSimulationClientResponse response = await sender.Send(new Query(request));
                    return Results.Ok(response);
                })
                .Produces<GetPiSimulationClientResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "SearchPiSimulationClients",
                    Summary = "Search clients by simulation",
                    Description = "Search clients by simulation"
                })
                .WithName("SearchPiSimulationClients")
                .WithTags("Simulation");
            }
        }

        public record Query(GetPiSimulationClientRequest Request) : IRequest<GetPiSimulationClientResponse>
        {
            public long? Id { get; init; } = Request.Id;
            public string? SortBy { get; set; } = Request.SortBy;
            public bool? IsAscending { get; set; } = Request.IsAscending;
            public int? PageSize { get; init; } = Request.PageSize;
            public int? PageIndex { get; init; } = Request.PageIndex;
        }

        public class Handler(ISimulationPiService simulationPiService) : IRequestHandler<Query, GetPiSimulationClientResponse>
        {
            public async Task<GetPiSimulationClientResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                return await simulationPiService.GetPiSimulationClients(request.Id, request.SortBy, request.IsAscending, request.PageSize, request.PageIndex);
            }
        }
    }
}
