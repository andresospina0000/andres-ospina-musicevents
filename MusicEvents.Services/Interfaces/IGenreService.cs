using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Entities;

namespace MusicEvents.Services.Interfaces
{
    public interface IGenreService
    {
        Task<BaseResponseGeneric<ICollection<Genre>>> GetAsync();

        Task<BaseResponseGeneric<Genre>> GetAsync(int id);

        Task<BaseResponseGeneric<int>> CreateAsync(DtoGenre request);

        Task<BaseResponseGeneric<int>> UpdateAsync(int id, DtoGenre request);

        Task<BaseResponseGeneric<int>> DeleteAsync(int id);
    }
}
