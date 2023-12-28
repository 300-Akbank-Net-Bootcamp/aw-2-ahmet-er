using FluentValidation;
using Vb.Data.Entity;

namespace Vb.Api.Validations
{
    public class AccountTransactionValidator : AbstractValidator<AccountTransaction>
    {
        public AccountTransactionValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Account is required.");

            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage("Reference number is required.")
                .MaximumLength(50).WithMessage("Reference number must be 50 characters long.");

            RuleFor(x => x.TransactionDate)
                .NotEmpty().WithMessage("Transaction date is required.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .ScalePrecision(4, 18).WithMessage("Amount must have at most 4 decimal places");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(300).WithMessage("Description must be 300 characters long.");

            RuleFor(x => x.TransferType)
                .NotEmpty().WithMessage("Transfer type is required.")
                .MaximumLength(10).WithMessage("Transfer type must be 10 characters long.");
        }
    }
}
