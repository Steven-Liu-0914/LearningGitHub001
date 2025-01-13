using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Authorization;
using CACIB.CREW.Api.Features.Authorization.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CACIB.CREW.Api.Features.Authorization.Handlers
{
    public class ImportSchedules
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.AuthorizationRoutes.ImportSchedule, async (
                    [FromForm] ImportScheduleRequest request,
                    IMediator sender) =>
                {
                    ImportScheduleResponse response = await sender.Send(new ImportScheduleQuery(request));
                    return Results.Ok(response);
                })
                .Produces<IResult>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "ImportSchedule",
                    Summary = "Import schedules from an Excel file for given authorization",
                    Description = "Import schedules from an Excel file for given authorization"
                })
                .WithName("ImportSchedule")
                .WithTags("Authorization")
                .RequireAuthorization()
                .DisableAntiforgery();
            }
        }

        public record ImportScheduleQuery(ImportScheduleRequest Request) : IRequest<ImportScheduleResponse>
        {
            public string StartDate { get; set; } = Request.StartDate;
            public string EndDate { get; set; } = Request.EndDate;
            public decimal AuthorizedAmount { get; set; } = Request.AuthorizedAmount;
            public string AssignmentLevel { get; set; } = Request.AssignmentLevel;
            public int AssignmentLevelId { get; set; } = Request.AssignmentLevelId;
            public IFormFile File { get; set; } = Request.File;
        }

        public class Handler(IScheduleService scheduleService) : IRequestHandler<ImportScheduleQuery, ImportScheduleResponse>
        {
            public async Task<ImportScheduleResponse> Handle(ImportScheduleQuery query, CancellationToken cancellationToken)
            {
                return await scheduleService.ImportSchedules(query);
            }
        }
    }
}
