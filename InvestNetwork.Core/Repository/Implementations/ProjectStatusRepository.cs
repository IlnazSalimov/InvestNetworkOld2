using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class ProjectStatusRepository : IProjectStatusRepository
    {
        private IRepository<ProjectStatus> projectStatusRepository;

        public ProjectStatusRepository(IRepository<ProjectStatus> projectStatusRepository)
        {
            this.projectStatusRepository = projectStatusRepository;
        }

        public IQueryable<ProjectStatus> GetAll()
        {
            return projectStatusRepository.GetAll();
        }

        public ProjectStatus GetById(int id)
        {
            if (id == 0)
                return null;
            return projectStatusRepository.GetById(id);
        }

        public ProjectStatus GetByCode(int code)
        {
            if (code == 0)
                return null;
            return projectStatusRepository.GetAll().FirstOrDefault(s => s.StatusCode == code);
        }

        public void Insert(ProjectStatus model)
        {
            if (model == null)
                throw new ArgumentNullException("ProjectStatus");
            projectStatusRepository.Insert(model);
        }

        public void Update(ProjectStatus model)
        {
            if (model == null)
                throw new ArgumentNullException("ProjectStatus");
            projectStatusRepository.Update(model);
        }

        public void Delete(ProjectStatus model)
        {
            if (model == null)
                throw new ArgumentNullException("ProjectStatus");
            projectStatusRepository.Delete(model);
        }

        public void SaveChanges()
        {
            projectStatusRepository.SaveChanges();
        }
    }
}