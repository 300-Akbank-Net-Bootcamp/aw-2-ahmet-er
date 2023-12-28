using FluentValidation;
using Vb.Data.Entity;

namespace Vb.Api.Validations
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer is required.");

            RuleFor(x => x.ContactType)
                .NotEmpty().WithMessage("ContactType is required.")
                .MaximumLength(10).WithMessage("Contact type must be most 10 characters long.");

            RuleFor(x => x.Information)
                .NotEmpty().WithMessage("Information is required.")
                .MaximumLength(100).WithMessage("Information must be most 100 characters long.");

            RuleFor(x => x.IsDefault)
                .NotEmpty().WithMessage("IsDefault is required.");
        }
    }
}
