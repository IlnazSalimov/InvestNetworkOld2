using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу новостей проекта
    /// </summary>
    public interface IProjectNewsRepository
    {
        IQueryable<ProjectNew> GetAll();
        ProjectNew GetById(int id);
        int Insert(ProjectNew model);
        void Update(ProjectNew model);
        void Delete(ProjectNew model);
        void SaveChanges();
    }
}
