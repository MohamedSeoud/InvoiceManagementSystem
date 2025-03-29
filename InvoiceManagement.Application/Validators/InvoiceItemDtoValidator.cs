using FluentValidation;
using InvoiceManagement.Application.DTOs;

namespace InvoiceManagement.Application.Validators;

public class InvoiceItemDtoValidator : AbstractValidator<InvoiceItemDto>
{
    public InvoiceItemDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0");
    }
}