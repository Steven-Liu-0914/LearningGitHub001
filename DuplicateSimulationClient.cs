using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.StoredClients;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Simulation.Handlers;

public class DuplicateSimulationClient
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(ApiRouteConstants.SimulationPiRoutes.DuplicateSimulationClient, async (DuplicateSimulationClientRequest request, HttpContext context, IMediator sender) =>
            {
                BaseResponse response = await sender.Send(new Request(request));
                return Results.Ok(response);
            })
            .Produces<BaseResponse> (StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "DuplicateSimulationClient",
                Summary = "Duplicate PI simulation clients endpoint",
                Description = "Duplicate PI simulation clients"
            })
            .WithName("DuplicateSimulationClient")
            .WithTags("Simulation");
        }
    }

    public record Request : IRequest<BaseResponse>
    {
        public DuplicateSimulationClientRequest RequestData { get; set; }
        public Request(DuplicateSimulationClientRequest requestData)
        {
            RequestData = requestData;
        }
    }

    public class Handler(IStoredClientService storedClientService) : IRequestHandler<Request, BaseResponse>
    {
        private readonly IStoredClientService _storedClientService = storedClientService;
        public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _storedClientService.DuplicateSimulationClientAsync(request.RequestData);
        }
    }
}
