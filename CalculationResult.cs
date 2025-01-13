using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.AuthorizationService;
using CACIB.CREW.Api.Core.Service.Calculation;
using CACIB.CREW.Api.Core.Service.Simulation;
using CACIB.CREW.Api.Features.Calculation.Model;
using CACIB.CREW.Api.Features.Simulation.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.Calculation.Handlers
{
    public class CalculationResult
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.CalculationRoutes.GetCalculationResult, async (int deviceId, IMediator sender) =>
                {
                    CalculationResultResponse response = await sender.Send(new Query(deviceId));
                    return Results.Ok(response);
                })
                .Produces<CalculationResultResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetCalculationResult",
                    Summary = "Get Calculation Result By Device Id",
                    Description = "This is an API Used to Get Calculation Result By Device Id In CREW"
                })
                .WithName("Calculation")
                .WithTags("Calculation")
                .RequireAuthorization();
            }
        }

        public record Query(int DeviceId) : IRequest<CalculationResultResponse>
        {
            public int DeviceId { get; init; } = DeviceId;
        }

        public class Handler(ICalculationService calculationService) : IRequestHandler<Query, CalculationResultResponse>
        {
            public async Task<CalculationResultResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                return await calculationService.GetCalculationResultByDeviceId(request.DeviceId);
            }
        }
    }
}
