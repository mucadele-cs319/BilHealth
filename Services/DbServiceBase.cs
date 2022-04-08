using BilHealth.Data;

namespace BilHealth.Services
{
    public abstract class DbServiceBase
    {
        protected readonly AppDbContext DbCtx;

        public DbServiceBase(AppDbContext dbCtx)
        {
            DbCtx = dbCtx;
        }
    }
}
