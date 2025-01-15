using CACIB.CREW.Api.Infrastructure;
using CREW.Core.Models.Response;

namespace CACIB.CREW.Api.Features.GeneralExport.Model
{

    public class ExportSearchSimulation : BaseResponse
    {
        public new List<ExportSimulationItem>? Data { get; set; }
    }

    public class ExportSimulationItem
    {
        public string? Date { get; set; }
        public long? ID { get; set; }
        public string? Type { get; set; }
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public string? Right { get; set; }
    }
}