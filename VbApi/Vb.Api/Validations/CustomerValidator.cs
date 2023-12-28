using FluentValidation;
using Vb.Data.Entity;

namespace Vb.Api.Validations
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.IdentityNumber)
                .NotEmpty().WithMessage("Identity number is required.")
                .Length(11).WithMessage("Identity number must be 11 ddigts long.")
                .Matches("^[0-9]*$").WithMessage("Identity number must contain only numeric digits.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name can be at most 50 characters long.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name can be at most 50 characters long.");

            RuleFor(x => x.CustomerNumber)
                .NotEmpty().WithMessage("Customer number is required.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be less than the current date.");

            RuleFor(x => x.LastActivityDate)
                .NotEmpty().WithMessage("Last activity date is required.");
        }
    }
}
