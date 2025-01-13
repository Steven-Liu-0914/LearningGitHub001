using CACIB.CREW.Api.Core.Common;
using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Calculation;
using CACIB.CREW.Api.Features.Calculation.Model.GetPiMonthly;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Calculation.Handlers
{
    public class GetPiMonthly
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.CalculationRoutes.GetPiMonthly, async ([AsParameters] GetPiMonthlyRequest getPiMonthlyRequest, IMediator sender) =>
                {
                    GetPiMonthlyResponse response = await sender.Send(new Query(getPiMonthlyRequest));
                    return Results.Ok(response);
                })
                .Produces<GetPiMonthlyResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetPiMonthly",
                    Summary = "Get PI Monthly List By Device Id and Level",
                    Description = "This is an API Used to Get PI Monthly List By Device Id and Level In CREW"
                })
                .WithName("GetPiMonthly")
                .WithTags("Calculation")
                .RequireAuthorization();
            }

            public record Query(GetPiMonthlyRequest Request) : IRequest<GetPiMonthlyResponse>
            {
                public int DeviceId { get; set; } = Request.DeviceId;
                public string Level { get; set; } = Request.Level;
                public string? EntityId { get; set; } = Request.EntityId;
            }

            public class Handler(ICalculationService calculationService) : IRequestHandler<Query, GetPiMonthlyResponse>
            {
                public async Task<GetPiMonthlyResponse> Handle(Query request, CancellationToken cancellationToken)
                {
                    return await calculationService.GetPiMonthlyListByDeviceIdAndLevel(request);
                }
            }
        }
    }
}
