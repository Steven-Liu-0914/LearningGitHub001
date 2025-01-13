using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Mitigants;
using CACIB.CREW.Api.Features.Mitigants.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Mitigants.Handlers
{
    public class GetMitigantAssignments
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.MitigantRoutes.GetMitigantAssignments, async ([AsParameters] GetMitigantAssignmentsRequest request, IMediator sender) =>
                {
                    GetMitigantAssignmentsResponse response = await sender.Send(new GetMitigantAssignmentQuery(request));
                    return Results.Ok(response);
                })
                .Produces<GetMitigantAssignmentsResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetMitigantAssignments",
                    Summary = "Get list of mitigant assignments In CREW",
                    Description = "Get list of mitigant assignments In CREW"
                })
                .WithName("GetMitigantAssignments")
                .WithTags("Mitigants");
            }
        }

        public record GetMitigantAssignmentQuery(GetMitigantAssignmentsRequest Request) : IRequest<GetMitigantAssignmentsResponse>
        {
            public long? MitigantId { get; set; } = Request.MitigantId;
            public long? DeviceId { get; set; } = Request.DeviceId;
            public long? AuthorizationId { get; set; } = Request.AuthorizationId;
            public string SortBy { get; set; } = Request.SortBy ?? "id";
            public bool IsAscending { get; set; } = Request.IsAscending ?? false;
            public int PageSize { get; set; } = Request.PageSize ?? 20;
            public int PageIndex { get; set; } = Request.PageIndex ?? 0;
        }

        public class Handler(IMitigantService mitigantService) : IRequestHandler<GetMitigantAssignmentQuery, GetMitigantAssignmentsResponse>
        {
            public async Task<GetMitigantAssignmentsResponse> Handle(GetMitigantAssignmentQuery query, CancellationToken cancellationToken)
            {
                return await mitigantService.GetMitigantAssignments(query);
            }
        }
    }
}
