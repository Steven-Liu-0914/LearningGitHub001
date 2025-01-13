using FluentValidation;
using static CACIB.CREW.Api.Features.Authorization.Handlers.ImportSchedules;

namespace CACIB.CREW.Api.Features.Authorization.Validators
{
    public class ImportSchedulesValidator : AbstractValidator<ImportScheduleQuery>
    {
        public ImportSchedulesValidator()
        {
            RuleFor(x => x.File).NotEmpty();
        }
    }
}
