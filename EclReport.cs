using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Calculation;
using CACIB.CREW.Api.Features.Calculation.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Calculation.Handlers;

public class EclReport
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.CalculationRoutes.GetEclReport, async ([AsParameters] EclReportRequest request, IMediator sender) =>
            {
                EclReportResponse response = await sender.Send(new Query(request));
                return Results.Ok(response);
            })
            .Produces<EclReportResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetEclReport",
                Summary = "Get ECL report for export",
                Description = "Get ECL report for export"
            })
            .WithName("GetEclReport")
            .WithTags("Calculation");
        }
    }

    public record Query : IRequest<EclReportResponse>
    {
        public EclReportRequest Request { get; set; }
        public Query(EclReportRequest request) => Request = request;
    }

    public class Handler(IEclService eclService) : IRequestHandler<Query, EclReportResponse>
    {
        public async Task<EclReportResponse> Handle(Query query, CancellationToken cancellationToken)
        {
            return await eclService.GetEclReport(query.Request);
        }
    }
}
