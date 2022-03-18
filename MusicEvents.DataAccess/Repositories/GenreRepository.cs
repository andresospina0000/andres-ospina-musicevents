using AutoMapper;
using MusicEvents.Entities;

namespace MusicEvents.DataAccess.Repositories
{
    public class GenreRepository : MusicContextBase<Genre>, IGenreRepository
    {
        public GenreRepository(MusicalEventsDbContext context, IMapper mapper) 
            : base(context, mapper)
        {

        }

        public async Task<ICollection<Genre>> GetCollectionAsync()
        {
            return await Context.SelectAsync<Genre>();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await Context.SelectAsync<Genre>(id);
        }

        public async Task<int> CreateAsync(Genre genre)
        {
            return await Context.InsertAsync(genre);
        }

        public async Task<int> UpdateAsync(Genre genre)
        {
            await Context.UpdateAsync(genre);

            return genre.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            await Context.DeleteAsync(new Genre
            {
                Id = id
            });

            return id;
        }

    }
}
