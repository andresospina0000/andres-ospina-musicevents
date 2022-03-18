using MusicEvents.Entities;
using MusicEvents.Entities.Complex;

namespace MusicEvents.DataAccess.Repositories
{
    public interface IEventRepository 
    {
        Task<(ICollection<ConcertInfo> Collection, int Total)> GetCollectionAsync(string filter, int page, int rows);

        Task<ICollection<ConcertInfo>> GetCollectionByGenre(int id);
        Task<ICollection<ConcertMinimalInfo>> GetMinimalCollectionByGenre(int id);

        Task<Concert> GetByIdAsync(int id);

        Task<int> CreateAsync(Concert concert);

        Task<int> UpdateAsync(Concert concert);

        Task Finalize(int id);

        Task<int> DeleteAsync(int id);
    }
}

