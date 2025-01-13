using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Mitigants;
using CACIB.CREW.Api.Features.Mitigants.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Mitigants.Handlers
{
    public class GetMitigantDetails
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapGet(ApiRouteConstants.MitigantRoutes.GetMitigantDetails, async (int mitigantId, IMediator sender) =>
                {
                    GetMitigantDetailsResponse response = await sender.Send(new GetMitigantDetailsQuery(mitigantId));
                    return Results.Ok(response);
                })
                .Produces<GetMitigantDetailsResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetMitigantDetails",
                    Summary = "Get mitigant details for given mitigant id",
                    Description = "This is an API Used to get mitigant details for given mitigant id in CREW"
                })
                .WithName("GetMitigantDetails")
                .WithTags("Mitigants")
                .RequireAuthorization();
            }
        }

        public record GetMitigantDetailsQuery(int MitigantId) : IRequest<GetMitigantDetailsResponse>
        {
        }

        public class Handler(IMitigantService srv) : IRequestHandler<GetMitigantDetailsQuery, GetMitigantDetailsResponse>
        {
            public Task<GetMitigantDetailsResponse> Handle(GetMitigantDetailsQuery request, CancellationToken cancellationToken)
            {
                return srv.GetMitigantDetails(request.MitigantId);
            }
        }
    }
}
