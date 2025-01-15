using CACIB.CREW.Api.Infrastructure;

namespace CACIB.CREW.Api.Features.GeneralExport.Model
{
    public class ExportPiSimulationAuthorization : BaseResponse
    {
        public new IEnumerable<ExportSimulationAuthorizationItem>? Data { get; set; }
    }

    public class ExportSimulationAuthorizationItem
    {
        public long Id { get; set; }
        public string? Type { get; set; }
        public string? ShortName { get; set; }
        public string? KorusId { get; set; }
        public string? KycId { get; set; }
        public string? Status { get; set; }
        public int? Entity { get; set; }
        public string? KorusRating { get; set; }
        public DateTime? KorusRatingDate { get; set; }
        public string? Ncf { get; set; }
        public string? Npa { get; set; }
        public string? Nor { get; set; }
        public DateTime? DecisionRatingDate { get; set; }
        public string? RatingMethodology { get; set; }
        public int? RatingReason { get; set; }
        public string? IndividualRisk { get; set; }
        public int? BiiPortfolioId { get; set; }
        public long AuthorizationId { get; set; }
        public string? InCalculation { get; set; }
        public string? ClientName { get; set; }
        public string? SubGroup { get; set; }
        public long? CphId { get; set; }
        public string? GroupName { get; set; }
        public string? MainProductId { get; set; }
        public string? ProductLoanscape { get; set; }
        public string? Amount { get; set; }
        public string? AmountCurrency { get; set; }
        public bool IsFrozen { get; set; }
        public string? DurationValue { get; set; }
        public string? DurationUnit { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? ProductTenor { get; set; }
        public int ProductTenorUnitId { get; set; }
        public string? LineSpecificId { get; set; }
        public bool? IsProcessed { get; set; }
        public string? HasSyndication { get; set; }
        public string? HasMitigant { get; set; }
        public string? HasBridle { get; set; }
        public string? DealName { get; set; }
        public string? NetPi { get; set; }
        public string? EvaBrut { get; set; }
        public string? Rwa { get; set; }
        public string? Ecl { get; set; }
    }
}
