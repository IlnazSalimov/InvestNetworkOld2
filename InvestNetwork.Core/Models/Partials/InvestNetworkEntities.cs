using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public partial class InvestNetworkEntities : IDataContext
    {
        DbEntityEntry<TEntity> IDataContext.Entry<TEntity>(TEntity entity)
        {
            return this.Entry(entity);
        }

        IDbSet<TEntity> IDataContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        int IDataContext.SaveChanges()
        {
            return this.SaveChanges();
        }

        void IDataContext.Dispose()
        {
            this.Dispose();
        }
    }
}