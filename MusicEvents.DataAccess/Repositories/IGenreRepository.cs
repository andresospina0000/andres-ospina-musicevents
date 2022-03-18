using Microsoft.EntityFrameworkCore;
using MusicEvents.Entities;

namespace MusicEvents.DataAccess.Repositories
{
    public interface IGenreRepository
    {
        Task<ICollection<Genre>> GetCollectionAsync();

        Task<Genre> GetByIdAsync(int id);

        Task<int> CreateAsync(Genre genre);

        Task<int> UpdateAsync(Genre genre);

        Task<int> DeleteAsync(int id);
    }
}
