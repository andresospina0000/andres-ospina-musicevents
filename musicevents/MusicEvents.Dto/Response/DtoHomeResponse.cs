using MusicEvents.Entities;
using MusicEvents.Entities.Complex;

namespace MusicEvents.Dto.Response;

public class DtoHomeResponse
{
    public BaseResponseGeneric<ICollection<Genre>> Genres { get; set; }

    public BaseResponseGeneric<ICollection<ConcertInfo>> Events { get; set; }

}