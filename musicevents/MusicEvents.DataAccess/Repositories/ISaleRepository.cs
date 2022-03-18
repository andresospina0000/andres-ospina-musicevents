using MusicEvents.Entities;
using MusicEvents.Entities.Complex;

namespace MusicEvents.DataAccess.Repositories;

public interface ISaleRepository
{
    Task<int> CreateAsync(Sale entity);

    Task<ICollection<SaleInfo>> GetSaleById(int id);
    Task<ICollection<SaleInfo>> GetSaleCollection(int genreId, DateTime? dateInit, DateTime? dateEnd);

    Task<ICollection<SaleInfo>> GetSaleByUserId(string userId);

    Task<ICollection<ReportSaleInfo>> GetReportSale(int genreId, DateTime dateInit, DateTime dateEnd);

}