using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.AuthorizationService;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class GetAuthorizationsForSimulation
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.SimulationRoutes.GetAuthorizations, async ([AsParameters] GetAuthorizationsRequest request, IMediator sender) =>
                {
                    GetAuthorizationsResponse response = await sender.Send(new GetAuthorizationsForSimulationQuery(request));
                    return Results.Ok(response);
                })
                .Produces<GetAuthorizationsResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetAuthorizationsList",
                    Summary = "Get authorization list",
                    Description = "Get authorization list for given device id"
                })
                .WithName("GetAuthorizations")
                .WithTags("Simulation");
            }
        }

        public record GetAuthorizationsForSimulationQuery(GetAuthorizationsRequest Request) : IRequest<GetAuthorizationsResponse>
        {
            public long Id { get; init; } = Request.Id;
            public string SortBy { get; set; } = Request.SortBy ?? "id";
            public bool IsAscending { get; set; } = Request.IsAscending ?? false;
            public int PageSize { get; init; } = Request.PageSize ?? 20;
            public int PageIndex { get; init; } = Request.PageIndex ?? 0;
        }

        public class Handler(IEuAuthorizationService srv) : IRequestHandler<GetAuthorizationsForSimulationQuery, GetAuthorizationsResponse>
        {
            private readonly IEuAuthorizationService _srv = srv;

            public async Task<GetAuthorizationsResponse> Handle(GetAuthorizationsForSimulationQuery query, CancellationToken cancellationToken)
            {
                return await _srv.GetAuthorizationsForSimulation(query);
            }
        }
    }
}