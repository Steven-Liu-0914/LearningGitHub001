using CACIB.CREW.Api.Infrastructure;

namespace CACIB.CREW.Api.Features.GeneralExport.Model
{
    public class ExportPiSimulationClient : BaseResponse
    {
        public new List<ExportPiSimulationClientItem>? Data { get; set; }
    }

    public class ExportPiSimulationClientItem
    {
        public string? Type { get; set; }
        public string? ActionUser { get; set; }
        public string? ShortName { get; set; }
        public string? KorusId { get; set; }
        public string? KycId { get; set; }
        public string? Status { get; set; }
        public string? Entity { get; set; }
        public string? FinalInternalRating { get; set; }
        public string? RatingDate { get; set; }
        public string? Ncf { get; set; }
        public string? Npa { get; set; }
        public string? Nor { get; set; }
        public string? DecisionRatingDate { get; set; }
        public string? RatingMethodology { get; set; }
        public string? RatingReason { get; set; }
        public string? IndividualRisk { get; set; }
        public string? BaselPortfolio { get; set; }
        public string? NetPi { get; set; }
        public string? EvaGross { get; set; }
        public string? Rwa { get; set; }
        public string? Ecl { get; set; }
    }
}
