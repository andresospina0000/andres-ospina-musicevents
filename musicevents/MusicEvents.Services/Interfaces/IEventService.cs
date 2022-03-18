using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Entities;
using MusicEvents.Entities.Complex;

namespace MusicEvents.Services.Interfaces;

public interface IEventService
{
    Task<BaseCollectionResponse<ICollection<ConcertInfo>>> GetAsync(string filter, int page, int rows);
    
    Task<BaseResponseGeneric<ICollection<ConcertInfo>>> GetByGenreAsync(int genreId);
    Task<BaseResponseGeneric<ICollection<ConcertMinimalInfo>>> GetMinimalByGenreAsync(int genreId);

    Task<BaseResponseGeneric<Concert>> GetAsync(int id);

    Task<BaseResponseGeneric<int>> CreateAsync(DtoEvent request);

    Task<BaseResponse> FinalizeAsync(int id);

    Task<BaseResponseGeneric<int>> UpdateAsync(int id, DtoEvent request);

    Task<BaseResponseGeneric<int>> DeleteAsync(int id);
}