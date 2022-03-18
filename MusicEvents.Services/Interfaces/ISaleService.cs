using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Entities.Complex;

namespace MusicEvents.Services.Interfaces;

public interface ISaleService
{
    Task<BaseResponseGeneric<int>> CreateAsync(DtoSale request, string userId);

    Task<BaseResponseGeneric<ICollection<DtoSaleInfo>>> GetSaleById(int id);
    Task<BaseResponseGeneric<ICollection<DtoSaleInfo>>> GetCollection(int genreId, string dateInit, string dateEnd);
    Task<BaseResponseGeneric<ICollection<DtoSaleInfo>>> GetSaleByUserId(string userId);

    Task<BaseResponseGeneric<ICollection<ReportSaleInfo>>> GetReportSales(int genreId, string dateInit, string dateEnd);

}