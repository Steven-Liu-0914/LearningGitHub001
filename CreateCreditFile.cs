using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Core.Service.CreditFileService;
using CACIB.CREW.Api.Core.Service.UserAccess;
using CACIB.CREW.Api.Features.CreditFile.Model;
using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Infrastructure.Models;
using MediatR;

namespace CACIB.CREW.Api.Features.CreditFile.Handlers
{
    public class CreateCreditFile
    {
        public class Endpoint : IEndpoint
        {
            public void ConfigureRoute(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapPost(ApiRouteConstants.CreditFileRoutes.CreateCreditFile, async (CreditFileRequest request, IMediator sender) =>
                {
                    CreditFileResponse response = await sender.Send(new Query(request));
                    return Results.Ok(response);
                })
                .Produces<CreditFileResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "CreateCreditFile",
                    Summary = "Create Credit File",
                    Description = "This is an API Used to Create Creditfile In CREW"
                })
                .WithName("CreditFile")
                .WithTags("CreditFile")
                .RequireAuthorization();
            }
        }

        public record Query(CreditFileRequest request) : IRequest<CreditFileResponse>
        {
            public string Name { get; set; } = request.Name;
            public bool InsiderList { get; set; } = request.InsiderList;
            public string? ConfidentialityLevel { get; set; } = request.ConfidentialityLevel;
            public string RequestType { get; set; } = request.RequestType;
            public string Requestformat { get; set; } = request.RequestFormat;
            public string BeneficiaryTypeMain { get; set; } = request.BeneficiaryTypeMain;
            public string WorkflowStatus { get; set; } = request.WorkflowStatus;
            public string? CountrySearchType { get; set; } = request.CountrySearchType;
            public List<string>? FilterCountryCodes { get; set; } = request.FilterCountryCodes;
            public string? FilterOption { get; set; } = request.FilterOption;
            public List<string>? FilterCategories { get; set; } = request.FilterCategories;
            public string? ShowIsolatedCounterparties { get; set; } = request.ShowIsolatedCounterparties;
            public string? BlockReviewId { get; set; } = request.BlockReviewId;
            public string? BlockReviewName { get; set; } = request.BlockReviewName;
            public List<BeneficiaryItem> Beneficiaries { get; set; } = request.Beneficiaries;
        }

        public class Handler(ICreditFileService creditFileService, IAuthTokenService authTokenService) : IRequestHandler<Query, CreditFileResponse>
        {
            public async Task<CreditFileResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                request.request.UtCode = authTokenService.GetUserUtCode();
                return await creditFileService.CreateNewCreditFile(request.request);
            }
        }
    }
}
