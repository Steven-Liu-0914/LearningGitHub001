using CACIB.CREW.Api.Infrastructure;

namespace CACIB.CREW.Api.Features.GeneralExport.Model
{
    public class ExportPiSimulationSchedule : BaseResponse
    {
        public new IList<ExportPiSimulationScheduleItem>? Data { get; set; }
    }


    public class ExportPiSimulationScheduleItem
    {
        public DateTime? DueDate { get; set; }
        public decimal? DueAmount { get; set; }
        public decimal? AuthorizedAmount { get; set; }
        public string? Type { get; set; }
        public string? AuthorizedAmountCurrency { get; set; }
        public decimal? UtilizationForecast { get; set; }
        public decimal? Margin { get; set; }
        public decimal? PercentageOfUse { get; set; }
        public decimal? CommitmentFees { get; set; }
    }
}