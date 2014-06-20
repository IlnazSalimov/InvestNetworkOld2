using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InvestNetwork.Core
{
    public partial class InvestNetworkEntities : IDataContext
    {
        DbEntityEntry<TEntity> IDataContext.Entry<TEntity>(TEntity entity)
        {
            return this.Entry(entity);
        }

        DbSet<TEntity> IDataContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        int SaveChanges()
        {
            return this.SaveChanges();
        }

        Task<int> SaveChangesAsync()
        {
            return this.SaveChangesAsync();
        }

        void IDataContext.Dispose()
        {
            this.Dispose();
        }
    }
}