using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
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
