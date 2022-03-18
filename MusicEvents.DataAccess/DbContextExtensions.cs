using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicEvents.Entities;

namespace MusicEvents.DataAccess
{
    public static class DbContextExtensions
    {

        public static async Task<ICollection<TEntityBase>> SelectAsync<TEntityBase>(this DbContext context)
            where TEntityBase : EntityBase
        {
            return await context.Set<TEntityBase>()
                .Where(p => p.Status)
                .AsNoTracking()
                .ToListAsync();
        }


        public static async Task<TEntityBase> SelectAsync<TEntityBase>(this DbContext context, int id)
            where TEntityBase : EntityBase
        {
            return await context.Set<TEntityBase>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Status);
        }

        public static async Task<int> InsertAsync<TEntityBase>(this DbContext context, TEntityBase entity)
            where TEntityBase : EntityBase
        {
            await context.Set<TEntityBase>().AddAsync(entity);
            context.Entry(entity).State = EntityState.Added;

            bool success = await context.SaveChangesAsync() > 0;

            return success ? entity.Id : 0;   
        }

        public static async Task UpdateAsync<TEntityBase>(this DbContext context, TEntityBase entity)
            where TEntityBase : EntityBase
        {
            context.Set<TEntityBase>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public static async Task DeleteAsync<TEntityBase>(this DbContext context, TEntityBase entity)
            where TEntityBase : EntityBase
        {
            var registro = await context.Set<TEntityBase>().FindAsync(entity.Id);
            if (registro != null)
            {
                registro.Status = false;
            }
            await context.SaveChangesAsync();
        }
    }
}
