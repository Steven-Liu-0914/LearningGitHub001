using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Calculation;
using CACIB.CREW.Api.Features.Calculation.Model.PI;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Calculation.Handlers
{
    public class GetPiDetailReport
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.CalculationRoutes.GetPiDetailReport, async ([AsParameters] PiDetailRequest request, IMediator sender) =>
                {
                    PiDetailReportResponse response = await sender.Send(new Query(request));
                    return Results.Ok(response);
                })
                .Produces<PiDetailReportResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetPiDetailReport",
                    Summary = "Get PI Detail Report at different levels",
                    Description = "Get PI Detail Report at different levels"
                })
                .WithName("GetPiDetailReport")
                .WithTags("Calculation");
            }
        }

        public record Query(PiDetailRequest Request) : IRequest<PiDetailReportResponse>
        {

        }

        public class Handler(ICalculationService calculationService) : IRequestHandler<Query, PiDetailReportResponse>
        {
            public async Task<PiDetailReportResponse> Handle(Query query, CancellationToken cancellationToken)
            {
                return await calculationService.GetPiDetailReportByLevel(query.Request);
            }
        }
    }
}
