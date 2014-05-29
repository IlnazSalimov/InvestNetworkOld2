using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    public interface IProjectCommentRepository
    {
        IQueryable<ProjectComment> GetAll();
        ProjectComment GetById(int id);
        IQueryable<ProjectComment> GetByProjectId(int id);
        void Insert(ProjectComment model);
        void Update(ProjectComment model);
        void Delete(ProjectComment model);
        void SaveChanges();
    }
}
