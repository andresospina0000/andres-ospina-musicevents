using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicEvents.Entities;
using MusicEvents.Entities.Complex;

namespace MusicEvents.DataAccess.Repositories;

public class EventRepository : MusicContextBase<Concert>, IEventRepository
{
    public EventRepository(MusicalEventsDbContext context, IMapper mapper)
    : base(context, mapper)
    {

    }
    public async Task<(ICollection<ConcertInfo> Collection, int Total)> GetCollectionAsync(string filter, int page, int rows)
    {
        Expression<Func<Concert, bool>> predicate = p => p.Title.Contains(filter ?? string.Empty);

        /*
         * LINQ - EAGER LOADING
         * TE TRAE TODA LA TABLA RELACIONADA
         * LINQ - LAZY LOADING
         * TE TRAE SOLO LA DATA QUE NECESITAS.
         */

        var query = await Context.Set<Concert>()
            .Where(predicate)
            .OrderBy(p => p.DateEvent)
            .AsNoTracking()
            .Select(x => new ConcertInfo
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DateEvent = x.DateEvent.ToLongDateString(),
                Genre = x.Genre.Description,
                Status = x.Status ? "Habilitado" : "Inhabilitado",
                Place = x.Place,
                TicketsQuantity = x.TicketsQuantity,
                UnitPrice = x.UnitPrice,
                ImageUrl = x.ImageUrl
            })
            .Skip((page - 1) * rows)
            .Take(rows)
            .ToListAsync();

        var totalCount = await Context.Set<Concert>()
            .Where(predicate)
            .AsNoTracking()
            .CountAsync();

        return (query, totalCount);

    }

    public async Task<ICollection<ConcertInfo>> GetCollectionByGenre(int id)
    {
        return await Context.Set<Concert>()
            .Where(p => p.GenreId == id)
            .AsNoTracking()
            .OrderBy(p => p.DateEvent)
            .Select(x => new ConcertInfo
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DateEvent = x.DateEvent.ToLongDateString(),
                Genre = x.Genre.Description,
                Place = x.Place,
                Status = x.Status ? "Habilitado" : "Inhabilitado",
                TicketsQuantity = x.TicketsQuantity,
                UnitPrice = x.UnitPrice,
                ImageUrl = x.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<ICollection<ConcertMinimalInfo>> GetMinimalCollectionByGenre(int id)
    {
        return await Context.Set<Concert>()
            .Where(p => p.GenreId == id)
            .AsNoTracking()
            .OrderBy(p => p.DateEvent)
            .Select(x => new ConcertMinimalInfo
            {
                Id = x.Id,
                Title = x.Title,
            })
            .ToListAsync();
    }

    public async Task<Concert> GetByIdAsync(int id)
    {
        return await Context.SelectAsync<Concert>(id);
    }

    public async Task<int> CreateAsync(Concert concert)
    {
        return await Context.InsertAsync(concert);
    }

    public async Task<int> UpdateAsync(Concert concert)
    {
        await Context.UpdateAsync(concert);
        return concert.Id;
    }

    public async Task Finalize(int id)
    {
        var entity = await Context.Set<Concert>()
            .FindAsync(id);

        if (entity != null) entity.Finalized = true;

        await Context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        await Context.DeleteAsync(new Concert
        {
            Id = id
        });

        return id;
    }
}