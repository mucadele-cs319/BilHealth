using BilHealth.Utility;
using Microsoft.EntityFrameworkCore;

namespace BilHealth.Data
{
    public static class EFCoreExtensions
    {
        public static async Task<TEntity> FindOrThrowAsync<TEntity>(this DbSet<TEntity> dbSet, Guid id) where TEntity : class
        {
            var obj = await dbSet.FindAsync(id);
            if (obj is null) throw new IdNotFoundException(typeof(TEntity), id);
            return obj;
        }

        public static async Task<object> FindOrThrowAsync(this DbContext dbCtx, Type type, Guid id)
        {
            var obj = await dbCtx.FindAsync(type, id);
            if (obj is null) throw new IdNotFoundException(type, id);
            return obj;
        }
    }
}
