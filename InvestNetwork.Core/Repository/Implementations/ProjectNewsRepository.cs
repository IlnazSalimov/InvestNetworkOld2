using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Предоставляет методы, которые предоставляют доступ к хранилищу новостей проекта
    /// </summary>
    public class ProjectNewsRepository : IProjectNewsRepository
    {
        private IRepository<ProjectNew> projectNewsRepository;

        public ProjectNewsRepository(IRepository<ProjectNew> projectNewsRepository)
        {
            this.projectNewsRepository = projectNewsRepository;
        }

        public IQueryable<ProjectNew> GetAll()
        {
            return projectNewsRepository.GetAll();
        }

        public ProjectNew GetById(int id)
        {
            if (id == 0)
                return null;
            return projectNewsRepository.GetById(id);
        }

        public int Insert(ProjectNew model)
        {
            if (model == null)
                throw new ArgumentNullException("projectNews");
            return projectNewsRepository.Insert(model);
        }

        public void Update(ProjectNew model)
        {
            if (model == null)
                throw new ArgumentNullException("projectNews");
            projectNewsRepository.Update(model);

        }

        public void Delete(ProjectNew model)
        {
            if (model == null)
                throw new ArgumentNullException("projectNews");
            projectNewsRepository.Delete(model);
        }

        public void SaveChanges()
        {
            projectNewsRepository.SaveChanges();
        }
    }
}