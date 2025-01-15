using CACIB.CREW.Api.Core.Common;
using CACIB.CREW.Api.Infrastructure;

namespace CACIB.CREW.Api.Features.GeneralExport.Model
{
    public class ExportSearchBeneficiary(IList<ExportBeneficiaryDto> datas, int cnt) : BaseResponse
    {
        public new IList<ExportBeneficiaryDto>? Data { get; set; } = datas;
    }

    public class ExportBeneficiaryDto
    {
        public long Id { get; set; }
        public string? Type { get; set; }
        public string? KorusId
        {
            get
            {
                if (Enum.TryParse<ClientType>(Type, out var clietnType))
                {
                    return clietnType switch
                    {
                        ClientType.Cpg or ClientType.Cpm or ClientType.Cpx or ClientType.Cph => Id.ToString()?.PadLeft(6, '0'),
                        _ => Id.ToString()?.PadLeft(10, '0')
                    };
                }
                return Id.ToString();
            }
        }
        public string? LongName { get; set; }
        public string? ShortName { get; set; }
        public string? LegalName { get; set; }
        public string? CphShortName { get; set; }
        public string? CphLongName { get; set; }
        public bool? IsCancelled { get; set; }
    }
}
