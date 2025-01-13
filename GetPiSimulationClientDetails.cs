using Azure;
using CACIB.CREW.Api.Core.Model.Client;
using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers
{
    public class GetPiSimulationClientDetails
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.SimulationPiRoutes.GetPiSimulationClientDetails, async ([AsParameters] GetPiSimulationClientDetailsRequest request, IMediator sender) =>
                {
                    GetPiSimulationClientDetailsResponse response = await sender.Send(new Query(request));
                    return Results.Ok(response);
                })
                .Produces<GetPiSimulationClientDetailsResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "RetrieveClientDetails",
                    Summary = "Retrive PI Simulation client details",
                    Description = "Retrive PI Simulation client details"
                })
                .WithName("RetrieveClientDetails")
                .WithTags("Simulation");
            }
        }

        public record Query(GetPiSimulationClientDetailsRequest Request) : IRequest<GetPiSimulationClientDetailsResponse>
        {
            public long? ClientId { get; init; } = Request.ClientId;
            public int? KorusId { get; init; } = Request.KorusId;
            public int? KycId { get; init; } = Request.KorusId;
        }

        public class Handler(ISimulationPiService simulationPiService) : IRequestHandler<Query, GetPiSimulationClientDetailsResponse>
        {
            public async Task<GetPiSimulationClientDetailsResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                return await simulationPiService.GetPiSimulationClientDetails(request.Request);
            }
        }
    }
}
