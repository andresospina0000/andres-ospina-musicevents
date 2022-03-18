using System.Data;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicEvents.Entities;
using MusicEvents.Entities.Complex;

namespace MusicEvents.DataAccess.Repositories;

public class SaleRepository : MusicContextBase<Sale>, ISaleRepository
{
    public SaleRepository(MusicalEventsDbContext context, IMapper mapper)
        : base(context, mapper)
    {

    }

    public async Task<int> CreateAsync(Sale entity)
    {
        var number = await Context.Set<Sale>()
            .AsNoTracking()
            .CountAsync();

        number++;

        entity.OperationNumber = $"{number:00000}";

        return await Context.InsertAsync(entity);
    }

    public async Task<ICollection<SaleInfo>> GetSaleById(int id)
    {
        var collection = Context.Set<SaleInfo>()
            .FromSqlRaw("EXEC uspSelectEventById {0}", id);

        return await collection.ToListAsync();
    }

    public async Task<ICollection<SaleInfo>> GetSaleCollection(int genreId, DateTime? dateInit, DateTime? dateEnd)
    {
        var collection = Context.Set<SaleInfo>()
            .FromSqlRaw("EXEC uspSelectEventCollection {0}, {1}, {2}", genreId, dateInit!, dateEnd!);

        return await collection.ToListAsync();
    }

    public async Task<ICollection<SaleInfo>> GetSaleByUserId(string userId)
    {
        var collection = Context.Set<SaleInfo>()
            .FromSqlRaw("EXEC uspSelectEventByUserId {0}", userId);

        return await collection.ToListAsync();
    }

    public async Task<ICollection<ReportSaleInfo>> GetReportSale(int genreId, DateTime dateInit, DateTime dateEnd)
    {

        var list = new List<ReportSaleInfo>();

        await using var connection = new SqlConnection(Context.Database.GetConnectionString());
        await using var command = connection.CreateCommand();

        command.CommandText = "uspReportSales";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@GenreId", genreId);
        command.Parameters.AddWithValue("@DateInit", dateInit);
        command.Parameters.AddWithValue("@DateEnd", dateEnd);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            list.Add(new ReportSaleInfo
            {
                Day = reader.GetInt32(0),
                TotalSale = reader.GetDecimal(1)
            });
        }

        return list;
    }
}