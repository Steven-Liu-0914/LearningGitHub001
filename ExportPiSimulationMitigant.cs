using CACIB.CREW.Api.Infrastructure;

namespace CACIB.CREW.Api.Features.GeneralExport.Model
{
    public class ExportPiSimulationMitigant : BaseResponse
    {
        public new IList<ExportPiSimulationMitigantItem>? Data { get; set; }
    }

    public class ExportPiSimulationMitigantItem
    {
        public int MitigantId { get; set; }
        public string? Type { get; set; }
        public decimal? Version { get; set; }
        public string? AssignmentLevel { get; set; }
        public string? GuarantorOrDepositoryName { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
    }
}
