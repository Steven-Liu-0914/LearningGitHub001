using Azure;
using CACIB.CREW.Api.Core.Service.PiCalculation;
using CACIB.CREW.Api.Features.Apl.Model.Apl.Response.Dto;
using CACIB.CREW.Api.Features.Calculation.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Models.Response;
using MediatR;
using static CACIB.CREW.Api.Core.Route.ApiRouteConstants;

namespace CACIB.CREW.Api.Features.Apl.Handler
{
    public class GetPiCalculationResult : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(CalculationRoutes.ComputePi, async (GetPiCalculationRequest request, IMediator sender) =>
            {
                return await sender.Send(new Query(request.CreditfileId, request.DeviceId));
            })
            .Produces<BaseResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "ComputePI",
                Summary = "Compute PI by DeviceId and CreditFileId",
                Description = "This is an API used to compute PI using DeviceId and CreditFileId In CREW"
            })
            .WithName("ComputePI")
            .WithTags("Calculation")
            .RequireAuthorization();

        }
    }

    public record Query(long CreditfileId, long DeviceId) : IRequest<BaseResponse> { }

    public class GetPiCalculationRequest
    {
        public long CreditfileId { get; set; }
        public long DeviceId { get; set; }
    }

    public class Handler(IPiCalculationService piCalculationService) : IRequestHandler<Query, BaseResponse>
    {
        public async Task<BaseResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var calculationResult = await piCalculationService.GetDeviceCalculationResult(request.CreditfileId, request.DeviceId);
            BaseResponse result = new();
            if (calculationResult == null)
            {
                RaiseError.CustomError("PI-CAL-001");              
            }
            else if ((calculationResult.SimulationError != null && !string.IsNullOrWhiteSpace(calculationResult.SimulationError.LeafType)))
            {
                RaiseError.CustomError("PI-CAL-002");
            }
            else
            {
                result.Data = true;
            }
            return result;
        }
    }
}
