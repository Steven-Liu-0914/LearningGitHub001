using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Data.Entities;
using CACIB.CREW.Api.Utility;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class SearchSimulation
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.SimulationRoutes.SearchSimulation, async ([AsParameters] SearchSimulationRequest request, IMediator sender) =>
                {
                    SearchSimulationResponse response = await sender.Send(new Query(request));
                    return Results.Ok(response);
                })
                .Produces<SearchSimulationResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "SearchSimulation",
                    Summary = "Get the simulation search results",
                    Description = "This is an API Used to search simulation in CREW"
                })
                .WithName("SearchSimulation")
                .WithTags("Simulation");
            }
        }

        public record Query(SearchSimulationRequest Request) : IRequest<SearchSimulationResponse>
        {
            public string? Name { get; init; } = Request.Name;
            public string? SortBy { get; set; } = Request.SortBy;
            public bool? IsAscending { get; set; } = Request.IsAscending;
            public int? PageSize { get; init; } = Request.PageSize;
            public int? PageIndex { get; init; } = Request.PageIndex;
            public string? SimulationType { get; set; } = Request.SimulationType;
            public int? BeneficiaryId { get; set; } = Request.BeneficiaryId;
            public string? BeneficiaryType { get; set; } = Request.BeneficiaryType;
        }

        public class Handler(ISimulationService simulationService) : IRequestHandler<Query, SearchSimulationResponse>
        {
            public async Task<SearchSimulationResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                return await simulationService.SearchSimulation(request.Name, request.SortBy, request.IsAscending, request.PageSize, request.PageIndex, request.SimulationType, request.BeneficiaryId, request.BeneficiaryType);
            }
        }
    }
}
