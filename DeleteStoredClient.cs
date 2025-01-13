#nullable disable

using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.StoredClients;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.StoredClient.Handler
{
    public class DeleteStoredClient
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapDelete(ApiRouteConstants.SimulationRoutes.DeleteStoredClient, async (int id, string type, HttpContext context, IMediator sender) =>
                {
                    BaseResponse response = await sender.Send(new Query(id, type));
                    return Results.Ok(response);
                })
                .Produces<BaseResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "DeleteStoredClient",
                    Summary = "Delete Selected Stored CLient",
                    Description = "This is an API used to delete stored client in CREW"
                })
                .WithName("DeleteStoredClient")
                .WithTags("Simulation")
                .RequireAuthorization();
            }
        }

        public record Query(int Id, string Type) : IRequest<BaseResponse>
        {
            public int Id { get; init; } = Id;
            public string ClientType { get; init; } = Type;
        }

        public class Handler(IStoredClientService storedClientService) : IRequestHandler<Query, BaseResponse>
        {
            private readonly IStoredClientService _storedClientService = storedClientService;

            public async Task<BaseResponse> Handle(Query request, CancellationToken cancellationToken)
            {

                HandlerResponse? response = await _storedClientService.DeleteStoredClient(request);

                return new BaseResponse { Data = true };

            }
        }
    }
}