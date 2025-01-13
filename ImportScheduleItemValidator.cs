using CACIB.CREW.Api.Core.Common;
using CACIB.CREW.Api.Features.Authorization.Model;
using FluentValidation;
using System.Globalization;
using static CACIB.CREW.Api.Features.Authorization.Handlers.ImportSchedules;

namespace CACIB.CREW.Api.Features.Authorization.Validators
{
    public class ImportScheduleItemValidator : AbstractValidator<ImportScheduleItem>
    {
        public ImportScheduleItemValidator()
        {
            RuleFor(x => x.DueDate)
                .Cascade(CascadeMode.Continue)
                .NotEmpty().WithMessage(ValidationErrorConsts.RequiredErrorConstant)
                .Custom((dueDateString, context) =>
                {
                    if (!DateTime.TryParseExact(dueDateString, "dd-MMM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dueDate))
                    {
                        context.AddFailure(ValidationErrorConsts.InvalidDateFormat);
                    }

                    var startDate = (DateTime)context.RootContextData[nameof(ImportScheduleQuery.StartDate)];
                    var endDate = (DateTime)context.RootContextData[nameof(ImportScheduleQuery.EndDate)];
                    var assignmentLevel = context.RootContextData[nameof(ImportScheduleQuery.AssignmentLevel)];
                    var assignmentLevelId = context.RootContextData[nameof(ImportScheduleQuery.AssignmentLevelId)];

                    if (dueDate < startDate || dueDate > endDate)
                    {
                        context.AddFailure(ValidationErrorConsts.InvalidDueDate.Replace("{x}", $"{assignmentLevel} {assignmentLevelId}"));
                    }
                });

            RuleFor(x => x.DueAmount)
                .Cascade(CascadeMode.Continue)
                .NotEmpty().WithMessage(ValidationErrorConsts.RequiredErrorConstant)
                .Custom((dueAmountString, context) =>
                {
                    if (!decimal.TryParse(dueAmountString, out decimal dueAmount))
                    {
                        context.AddFailure(ValidationErrorConsts.DueAmountNotNumeric);
                    }

                    if (dueAmount < 0)
                    {
                        context.AddFailure(ValidationErrorConsts.NegativeDueAmount);
                    }

                    var authorizedAmount = (decimal)context.RootContextData[nameof(ImportScheduleQuery.AuthorizedAmount)];
                    var assignmentLevel = context.RootContextData[nameof(ImportScheduleQuery.AssignmentLevel)];
                    var assignmentLevelId = context.RootContextData[nameof(ImportScheduleQuery.AssignmentLevelId)];

                    if (dueAmount > authorizedAmount)
                    {
                        context.AddFailure(ValidationErrorConsts.InvalidDueAmount.Replace("{x}", $"{assignmentLevel} {assignmentLevelId}"));
                    }
                });

            RuleFor(x => x.Type)
                .Cascade(CascadeMode.Continue)
                .NotEmpty().WithMessage(ValidationErrorConsts.RequiredErrorConstant)
                .Must(x => x!.Equals("R") || x.Equals("F")).WithMessage(ValidationErrorConsts.InvalidScheduleType);

            RuleFor(x => x.Percent)
                .Cascade(CascadeMode.Continue)
                .Must(x => decimal.TryParse(x, out decimal percentOfUse) && percentOfUse >= 0 && percentOfUse <= 100)
                .WithMessage(ValidationErrorConsts.InvalidPercent);

            RuleFor(x => x.AuthorizedAmount)
                .Cascade(CascadeMode.Continue)
                .Must(x => decimal.TryParse(x, out _)).WithMessage(ValidationErrorConsts.InvalidAuthorizedAmount);

            RuleFor(x => x.UtilizationForecast)
                .Cascade(CascadeMode.Continue)
                .Must(x => decimal.TryParse(x, out decimal utilizationForecast) && utilizationForecast >= 0 && utilizationForecast <= 100)
                .WithMessage(ValidationErrorConsts.InvalidUtilizationForecast);

            RuleFor(x => x.Margin)
                .Cascade(CascadeMode.Continue)
                .Must(x => decimal.TryParse(x, out _)).WithMessage(ValidationErrorConsts.MarginNotNumeric);

            RuleFor(x => x.CommitmentFees) 
                .Cascade(CascadeMode.Continue)
                .Must(x => decimal.TryParse(x, out _)).WithMessage(ValidationErrorConsts.CommitmentFeeNotNumeric);
        }
    }
}
