using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Infrastructure;
using MediatR;
using CACIB.CREW.Api.Features.Beneficiary.Model;
using CACIB.CREW.Api.Core.Service.Beneficiary;
using CACIB.CREW.Api.Features.ReferentialFilterable.Model;

namespace CACIB.CREW.Api.Features.Beneficiary.Handler.SearchBeneficiary
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.BeneficiaryRoutes.Search,
                ([AsParameters] SearchBeneficiaryRequest req, IMediator sender) =>
            sender.Send(new Request(req)))
            .Produces<FilterableReferentialResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "SearchBeneficiary",
                Summary = "Search beneficiary",
                Description = "Search beneficiary by type, korusId or name",
            })
            .WithName("SearchBeneficiary")
            .WithTags("Beneficiary");
        }
    }
    public record Request : IRequest<SearchBeneficiaryResponse>
    {
        public SearchBeneficiaryRequest Data { get; init; }
        public Request(SearchBeneficiaryRequest req)
        {
            Data = req;
        }
    }

    public class Handler(IBeneficiaryService srv) : IRequestHandler<Request, SearchBeneficiaryResponse>
    {
        readonly IBeneficiaryService _srv = srv;

        public async Task<SearchBeneficiaryResponse> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _srv.SearchBeneficiary(request.Data);
        }
    }
}
