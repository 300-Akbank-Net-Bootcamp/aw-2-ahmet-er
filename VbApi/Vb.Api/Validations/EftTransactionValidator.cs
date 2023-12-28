using FluentValidation;
using Vb.Data.Entity;

namespace Vb.Api.Validations
{
    public class EftTransactionValidator : AbstractValidator<EftTransaction>
    {
        public EftTransactionValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Account is required.");

            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage("Reference number is required.")
                .MaximumLength(50).WithMessage("Reference number can be at most 50 characters long.");

            RuleFor(x => x.TransactionDate)
                .NotEmpty().WithMessage("Transaction date is required.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required")
                .ScalePrecision(4, 18).WithMessage("Amount must have at most 4 decimal places.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Description can be at most 300 characters long.");

            RuleFor(x => x.SenderAccount)
                .NotEmpty().WithMessage("Sender account is required.")
                .MaximumLength(50).WithMessage("Sender account must have at most 50 characters long.");

            RuleFor(x => x.SenderIban)
                      .NotEmpty().WithMessage("Sender IBAN is required.")
                      .MaximumLength(50).WithMessage("Sender IBAN must have at most 50 characters long.");

            RuleFor(x => x.SenderName)
                      .NotEmpty().WithMessage("Sender name is required.")
                      .MaximumLength(50).WithMessage("Sender name must have at most 50 characters long.");
        }
    }
}
