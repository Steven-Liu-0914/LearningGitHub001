using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Mitigants;
using CACIB.CREW.Api.Features.Mitigants.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Mitigants.Handlers
{
    public class GetMitigants
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.MitigantRoutes.Mitigants, async ([AsParameters] GetMitigantsRequest request, IMediator sender) =>
                {
                    GetMitigantsResponse response = await sender.Send(new GetMitigantsQuery(request));
                    return Results.Ok(response);
                })
                .Produces<GetMitigantsResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetMitigants",
                    Summary = "Get mitigant list for given authorizationId or creditFileId",
                    Description = "This is an API Used to get mitigant list for given authorizationId or creditFileId in CREW"
                })
                .WithName("GetMitigants")
                .WithTags("Mitigants")
                .RequireAuthorization();
            }
        }

        public record GetMitigantsQuery(GetMitigantsRequest Request) : IRequest<GetMitigantsResponse>
        {
            public int? AuthorizationId = Request.AuthorizationId;
            public int DeviceId = Request.DeviceId;
            public bool ExcludeLinkedMitigants = Request.ExcludeLinkedMitigants;
            public string SortBy = Request.SortBy ?? "Id";
            public bool IsAscending = Request.IsAscending ?? false;
            public int PageSize = Request.PageSize ?? 20;
            public int PageIndex = Request.PageIndex ?? 0;
        }

        public class Handler(IMitigantService srv) : IRequestHandler<GetMitigantsQuery, GetMitigantsResponse>
        {
            public async Task<GetMitigantsResponse> Handle(GetMitigantsQuery request, CancellationToken cancellationToken)
            {
                return await srv.GetMitigants(request);
            }
        }
    }
}
