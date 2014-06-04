using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу статусов проекта
    /// </summary>
    public interface IProjectStatusRepository
    {
        IQueryable<ProjectStatus> GetAll();
        ProjectStatus GetById(int id);
        ProjectStatus GetByCode(int code);
        void Insert(ProjectStatus model);
        void Update(ProjectStatus model);
        void Delete(ProjectStatus model);
        void SaveChanges();
    }
}
