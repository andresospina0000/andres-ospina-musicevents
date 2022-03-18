using AutoMapper;
using MusicEvents.Entities;

namespace MusicEvents.DataAccess
{
    public class MusicContextBase<TEntityBase>
        where TEntityBase : EntityBase, new()
    {
        protected readonly MusicalEventsDbContext Context;
        public readonly IMapper Mapper;

        protected MusicContextBase(MusicalEventsDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
    }
}
