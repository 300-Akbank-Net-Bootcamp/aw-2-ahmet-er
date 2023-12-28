using FluentValidation;
using Vb.Data.Entity;

namespace Vb.Api.Validations
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer is required.");

            RuleFor(x => x.Address1)
                .NotEmpty().WithMessage("Address1 is required.")
                .MaximumLength(150).WithMessage("Address1 must be 150 characters long.");

            RuleFor(x => x.Address2)
                .MaximumLength(150).WithMessage("Address2 must be 150 characters long.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(100).WithMessage("Country must be 100 characters long.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City must be 100 characters long.");

            RuleFor(x => x.County)
                .MaximumLength(100).WithMessage("County must be 100 characters long.");

            RuleFor(x => x.PostalCode)
                .MaximumLength(10).WithMessage("Postal code must be 10 characters long.");

            RuleFor(x => x.IsDefault)
                .NotEmpty().WithMessage("IsDefault is required.");
        }
    }
}
