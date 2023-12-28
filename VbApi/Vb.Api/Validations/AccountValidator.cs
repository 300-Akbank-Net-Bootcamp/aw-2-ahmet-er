using FluentValidation;
using Vb.Data.Entity;

namespace Vb.Api.Validations
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer is required.");

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("Account number is required.");

            RuleFor(x => x.IBAN)
                .NotEmpty().WithMessage("IBAN is required.")
                .Length(34).WithMessage("IBAN must be 34 characters long.");

            RuleFor(x => x.Balance)
                .NotEmpty().WithMessage("Balance is required.")
                .ScalePrecision(4, 18).WithMessage("Balance must have at most 4 decimal places");

            RuleFor(x => x.CurrencyType)
                .NotEmpty().WithMessage("Currency type is required.")
                .MaximumLength(3).WithMessage("Currency type is must be 3 characters long.");

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Name is must be 100 characters long.");

            RuleFor(x => x.OpenDate)
                .NotEmpty().WithMessage("Open date is required.");
        }
    }
}
