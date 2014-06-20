using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу проектов
    /// </summary>
    public interface IProjectRepository
    {
        IQueryable<Project> GetAll();
        Project GetById(int id);
        int Insert(Project model);
        void Update(Project model);
        void Delete(Project model);
        void SaveChanges();

        Task<Project> GetByIdAsync(int id);

        IQueryable<Project> SearchFor(Expression<Func<Project, bool>> predicate);

        Task EditAsync(Project model);

        Task InsertAsync(Project model);

        Task DeleteAsync(Project model);
    }
}
