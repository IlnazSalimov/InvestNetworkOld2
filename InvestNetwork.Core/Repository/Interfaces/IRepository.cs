using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(object id);
        int Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void SaveChanges();
    }
}
