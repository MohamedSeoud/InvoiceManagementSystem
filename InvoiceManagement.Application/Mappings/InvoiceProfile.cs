using AutoMapper;
using InvoiceManagement.Application.DTOs;
using InvoiceManagement.Core.Entities;

namespace InvoiceManagement.Application.Mappings;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>().ReverseMap();
        CreateMap<InvoiceItem, InvoiceItemDto>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice)) // Ensure decimal calculation
            .ReverseMap();
    }
}
