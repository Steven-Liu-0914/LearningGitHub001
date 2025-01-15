using CACIB.CREW.Api.Core.Service.PiCalculation;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;
using static CACIB.CREW.Api.Core.Route.ApiRouteConstants;

namespace CACIB.CREW.Api.Features.Apl.Handler
{
    public class DeletePiCalculationFlag : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapDelete(CalculationRoutes.ComputePi, async (long deviceId, IMediator sender) =>
            {
                return await sender.Send(new Query(deviceId));
            })
            .Produces<BaseResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "UncomputePI",
                Summary = "Clear Device incalculation flag by DeviceId",
                Description = "This API removes in calculation flag of a Device"
            })
            .WithName("UnComputePI")
            .WithTags("Calculation")
            .RequireAuthorization();

        }
        public record Query(long DeviceId) : IRequest<BaseResponse>
        {
        }

        public class Handler(IPiCalculationService piCalculationService) : IRequestHandler<Query, BaseResponse>
        {
            public async Task<BaseResponse> Handle(Query query, CancellationToken cancellationToken)
            {
                return await piCalculationService.ClearDeviceCalculationFlag(query.DeviceId);
            }
        }
    }
}
