using CACIB.CREW.Api.Core.Service.AuthorizationService;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;
using static CACIB.CREW.Api.Core.Route.ApiRouteConstants;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class GetAuthorizationsForImport
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(SimulationRoutes.GetAuthorizationForImport, async ([AsParameters] ImportAuthorizationSearchRequest request, IMediator sender) =>
                {
                    ImportAuthorizationSearchResponse response = await sender.Send(new GetAuthorizationsForImportQuery(request));
                    return Results.Ok(response);
                })
                .Produces<ImportAuthorizationSearchResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetAuthorizations",
                    Summary = "Get Authorizations for import",
                    Description = "This is an API used to get Authorizations based on the Simulation Id in CREW"
                })
                .WithName("GetAuthorizationsImport")
                .WithTags("Simulation");
            }
        }

        public record GetAuthorizationsForImportQuery(ImportAuthorizationSearchRequest Request) : IRequest<ImportAuthorizationSearchResponse>
        {
            public long SimulationId { get; init; } = Request.Id;
            public string SortBy { get; init; } = Request.SortBy;
            public bool IsAscending { get; init; } = Request.IsAscending ?? false;
            public int PageSize { get; init; } = Request.PageSize ?? 20;
            public int PageIndex { get; init; } = Request.PageIndex ?? 0;
        }

        public class Handler(IEuAuthorizationService euAuthorizationService) : IRequestHandler<GetAuthorizationsForImportQuery, ImportAuthorizationSearchResponse>
        {
            public async Task<ImportAuthorizationSearchResponse> Handle(GetAuthorizationsForImportQuery query, CancellationToken cancellationToken)
            {
                return await euAuthorizationService.GetAuthorizationsForImport(query);
            }
        }
    }
}
