using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.Beneficiary;
using CACIB.CREW.Api.Features.Beneficiary.Model;
using CACIB.CREW.Api.Features.ReferentialFilterable.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.Beneficiary.Handler.SearchKyc
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.BeneficiaryRoutes.SearchKyc,
                ([AsParameters] SearchKycRequest req, IMediator sender) =>
            sender.Send(new Request(req)))
            .Produces<FilterableReferentialResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "SearchKyc",
                Summary = "Search Kyc",
                Description = "Search Kyc by kyc id or legal name",
            })
            .WithName("SearchKyc")
            .WithTags("Beneficiary");
        }
    }
    public record Request : IRequest<SearchKycResponse>
    {
        public SearchKycRequest Data { get; init; }
        public Request(SearchKycRequest req)
        {
            Data = req;
        }
    }

    public class Handler(IKycService srv) : IRequestHandler<Request, SearchKycResponse>
    {
        readonly IKycService _srv = srv;

        public async Task<SearchKycResponse> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _srv.SearchKyc(request.Data.KycId?.ToString(), request.Data.LegalName,
                request.Data.SortBy,request.Data.IsAscending,request.Data.PageIndex??0,
                request.Data.PageSize??0);
        }
    }
}
