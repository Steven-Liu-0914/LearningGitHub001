using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Beneficiary;
using CACIB.CREW.Api.Features.Beneficiary.Model;
using CACIB.CREW.Api.Features.ReferentialFilterable.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Beneficiary.Handler.GetBeneficiaryDetails
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.BeneficiaryRoutes.GetDetail,
                ([AsParameters] GetBeneficiaryDetailsRequest req, IMediator sender) =>
            sender.Send(new Request(req)))
            .Produces<GetBeneficiaryDetailsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "GetBeneficiaryDetails",
                Summary = "Get detail of beneficiary",
                Description = "Get detail of beneficiary by type, korusId or kycId",
            })
            .WithName("GetBeneficiaryDetails")
            .WithTags("Beneficiary");
        }
    }
    public record Request : IRequest<GetBeneficiaryDetailsResponse>
    {
        public GetBeneficiaryDetailsRequest Data { get; init; }
        public Request(GetBeneficiaryDetailsRequest req)
        {
            Data = req;
        }
    }

    public class Handler(IBeneficiaryService srv) : IRequestHandler<Request, GetBeneficiaryDetailsResponse>
    {
        readonly IBeneficiaryService _srv = srv;

        public async Task<GetBeneficiaryDetailsResponse> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _srv.GetBeneficiaryDetails(request.Data);
        }
    }
}
