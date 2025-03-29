using FluentValidation;
using InvoiceManagement.Application.DTOs;
using System.Linq;

namespace InvoiceManagement.Application.Validators;

public class InvoiceDtoValidator : AbstractValidator<InvoiceDto>
{
    public InvoiceDtoValidator()
    {
        RuleFor(x => x.InvoiceNumber)
            .NotEmpty().WithMessage("Invoice number is required")
            .MaximumLength(50).WithMessage("Invoice number cannot exceed 50 characters");

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Customer name is required")
            .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required")
            .Must(items => items.All(i => !string.IsNullOrWhiteSpace(i.Description)))
            .WithMessage("All items must have a description");

        RuleForEach(x => x.Items).SetValidator(new InvoiceItemDtoValidator());
    }
}