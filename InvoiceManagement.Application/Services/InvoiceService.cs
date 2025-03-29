using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using InvoiceManagement.Application.DTOs;
using InvoiceManagement.Application.Interfaces;
using InvoiceManagement.Core.Entities;
using InvoiceManagement.Core.Interfaces;


namespace InvoiceManagement.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<InvoiceDto> _invoiceValidator;

    public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<InvoiceDto> invoiceValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _invoiceValidator = invoiceValidator;
    }

    public async Task<InvoiceDto> GetInvoiceByIdAsync(int id)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetInvoiceWithItemsAsync(id);
        if (invoice == null)
            throw new KeyNotFoundException("Invoice not found");

        return _mapper.Map<InvoiceDto>(invoice);
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        var invoices = await _unitOfWork.InvoiceRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto)
    {
        // Manual validation (optional - FluentValidation should handle this)
        if (invoiceDto == null)
            throw new ArgumentNullException(nameof(invoiceDto));

        var validationResult = await _invoiceValidator.ValidateAsync(invoiceDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            throw new ValidationException("Invoice validation failed", (IEnumerable<ValidationFailure>)errors);
        }

        var invoice = _mapper.Map<Invoice>(invoiceDto);
        invoice.TotalAmount = invoice.Items.Sum(i => i.Quantity * i.UnitPrice);

        await _unitOfWork.InvoiceRepository.AddAsync(invoice);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<InvoiceDto>(invoice);
    }
    public async Task UpdateInvoiceAsync(InvoiceDto invoiceDto)
    {
        ValidationResult validationResult = await _invoiceValidator.ValidateAsync(invoiceDto);
        if (!validationResult.IsValid)
        {
            // Convert FluentValidation result to ValidationException
            var errors = validationResult.Errors
                .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage));
            throw new ValidationException(errors);
        }
        var existingInvoice = await _unitOfWork.InvoiceRepository.GetInvoiceWithItemsAsync(invoiceDto.Id);
        if (existingInvoice == null)
            throw new KeyNotFoundException("Invoice not found");

        _mapper.Map(invoiceDto, existingInvoice);
        existingInvoice.CalculateTotal();

        _unitOfWork.InvoiceRepository.UpdateAsync(existingInvoice);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteInvoiceAsync(int id)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id);
        if (invoice == null)
            throw new KeyNotFoundException("Invoice not found");

        await _unitOfWork.InvoiceRepository.DeleteAsync(invoice);
        await _unitOfWork.CompleteAsync();
    }
}