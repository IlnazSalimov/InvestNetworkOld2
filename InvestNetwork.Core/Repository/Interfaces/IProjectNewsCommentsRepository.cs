using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу комментариев, написанных к новости проекта
    /// </summary>
    public interface IProjectNewsCommentsRepository
    {
        List<ProjectNewsComment> GetAll();
        ProjectNewsComment GetById(int id);
        List<ProjectNewsComment> GetByProjectNewsId(int id);
        void Insert(ProjectNewsComment model);
        void Update(ProjectNewsComment model);
        void Delete(ProjectNewsComment model);
        void SaveChanges();
    }
}
