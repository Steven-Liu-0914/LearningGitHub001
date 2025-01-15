using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.DataExportService;
using CACIB.CREW.Api.Features.GeneralExport.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.GeneralExport.Handlers
{
    public class GeneralExport
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.GeneralExportRoutes.GeneralExport, async (GeneralExportRequest request, IMediator sender) =>
                {
                    IResult response = await sender.Send(new Query(request));
                    return Results.Ok(response);
                })
                .Produces<IResult>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GeneralExport",
                    Summary = "Get the export data",
                    Description = "This is an API Used to retrieve the export data in CREW"
                })
                .WithName("GeneralExport")
                .WithTags("GeneralExport");
            }
        }

        public record Query(GeneralExportRequest Request) : IRequest<IResult>
        {
            public string Category { get; set; } = Request.Category;
            public object SearchRequest { get; set; } = Request.SearchRequest;
            public List<HeaderInfo> HeaderItems { get; set; } = Request.HeaderItems;
        }

        public class Handler(IGeneralExportService generalExportService) : IRequestHandler<Query, IResult>
        {
            public async Task<IResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var exportDataResult = await generalExportService.GeneralExport(request.Request);
                if (exportDataResult is not null && exportDataResult.Item1 is not null && exportDataResult.Item2 is not null)
                {
                    return Results.File(exportDataResult.Item1, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataResult.Item2);
                }

                throw new InvalidOperationException("Failed to export data");
            }
        }
    }
}