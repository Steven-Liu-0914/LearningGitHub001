using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.CreditFileService;
using CACIB.CREW.Api.Features.CreditFile.Model;
using CACIB.CREW.Api.Infrastructure;
using MediatR;

namespace CACIB.CREW.Api.Features.CreditFile.Handlers;

public class SearchCreditFile
{
    public class Endpoint : IEndpoint
    {
        public void ConfigureRoute(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(ApiRouteConstants.CreditFileRoutes.SearchCreditFile, async ([AsParameters]SearchCreditFileRequest request, IMediator sender) =>
            {
                SearchCreditFileResponse response = await sender.Send(new SearchCreditFileQuery(request));
                return Results.Ok(response);
            })
            .Produces<SearchCreditFileResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                OperationId = "SearchForCreditFiles",
                Summary = "Search for credit files based on request criteria",
                Description = "Search for credit files based on request criteria"
            })
            .WithName("SearchCreditFiles")
            .WithTags("Credit file");
        }
    }

    public record SearchCreditFileQuery : IRequest<SearchCreditFileResponse>
    {
        public string Key { get; set; }
        public bool SearchFileName { get; set; }
        public bool SearchBeneficiary { get; set; }
        public bool SearchDealId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SortBy { get; set; }
        public bool? IsAscending { get; set; }
        public int? PageSize { get; init; }
        public int? PageIndex { get; init; }

        public SearchCreditFileQuery(SearchCreditFileRequest request)
        {
            Key = request.Key ?? string.Empty;
            SearchFileName = request.SearchFileName;
            SearchBeneficiary = request.SearchBeneficiary;
            SearchDealId = request.SearchDealId;
            StartDate = request.StartDate;
            EndDate = request.EndDate;
            SortBy = request.SortBy;
            IsAscending = request.IsAscending;
            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
        }
    }

    public class Handler(ISearchCreditFileService srv) : IRequestHandler<SearchCreditFileQuery, SearchCreditFileResponse>
    {
        public async Task<SearchCreditFileResponse> Handle(SearchCreditFileQuery query, CancellationToken cancellationToken)
        {
            return await srv.SearchCreditFiles(query);
        }
    }
}
