using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Mitigants;
using CACIB.CREW.Api.Features.Mitigants.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Mitigants.Handlers
{
    public class GetCreditFileForMitigant
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.MitigantRoutes.GetCreditFileForMitigant, async ([AsParameters] GetCreditFileDetailForMitigantRequest request, IMediator sender) =>
                {
                    GetCreditFileForMitigantResponse response = await sender.Send(new GetCreditFileForMitigantQuery(request));
                    return Results.Ok(response);
                })
                .Produces<GetCreditFileForMitigantResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetCreditFileDetailsWithAssignments",
                    Summary = "Get credit file data mitigants default assignments In CREW",
                    Description = "Get credit file data mitigants default assignments In CREW"
                })
                .WithName("GetCreditFileDetailsWithAssignments")
                .WithTags("Mitigants");
            }
        }

        public record GetCreditFileForMitigantQuery(GetCreditFileDetailForMitigantRequest Request) : IRequest<GetCreditFileForMitigantResponse>
        {
            public long? CreditFileId { get; set; } = Request.CreditFileId;
            public long? DeviceId { get; set; } = Request.DeviceId;
            public int? AuthorizationId { get; set; } = Request.AuthorizationId;
        }

        public class Handler(IMitigantService mitigantService) : IRequestHandler<GetCreditFileForMitigantQuery, GetCreditFileForMitigantResponse>
        {
            public async Task<GetCreditFileForMitigantResponse> Handle(GetCreditFileForMitigantQuery query, CancellationToken cancellationToken)
            {
                return await mitigantService.GetCreditFileForMitigant(query);
            }
        }
    }
}
