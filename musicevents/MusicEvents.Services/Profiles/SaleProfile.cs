using AutoMapper;
using MusicEvents.Dto.Response;
using MusicEvents.Entities;
using MusicEvents.Entities.Complex;

namespace MusicEvents.Services.Profiles;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<SaleInfo, DtoSaleInfo>()
            .ForMember(o => o.Id, d => d.MapFrom(x => x.Id))
            .ForMember(o => o.DateEvent, d => d.MapFrom(x => x.DateEvent.ToString(Constants.DateFormat)))
            .ForMember(o => o.TimeEvent, d => d.MapFrom(x => x.DateEvent.ToString(Constants.TimeFormat)))
            .ForMember(o => o.Quantity, d => d.MapFrom(x => x.Quantity))
            .ForMember(o => o.Title, d => d.MapFrom(x => x.Title))
            .ForMember(o => o.SaleDate, d => d.MapFrom(x => x.SaleDate.ToString(Constants.DateFormat)))
            .ForMember(o => o.SaleTime, d => d.MapFrom(x => x.SaleDate.ToString(Constants.TimeFormat)))
            .ForMember(o => o.TotalSale, d => d.MapFrom(x => x.TotalSale))
            .ForMember(o => o.Genre, d => d.MapFrom(x => x.Genre))
            .ForMember(o => o.OperationNumber, d => d.MapFrom(x => x.OperationNumber))
            .ForMember(o => o.FullName, d => d.MapFrom(x => x.FullName))
            .ForMember(o => o.ImageUrl, d => d.MapFrom(x => x.ImageUrl))
            .ReverseMap();

    }
}