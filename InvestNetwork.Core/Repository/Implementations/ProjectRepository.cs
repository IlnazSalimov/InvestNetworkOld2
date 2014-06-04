using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Предоставляет методы, которые предоставляют доступ к хранилищу проектов
    /// </summary>
    public class ProjectRepository : IProjectRepository
    {
        private IRepository<Project> projectRepository;
        private IProjectStatusRepository projectStatusRepository;

        public ProjectRepository(IRepository<Project> projectRepository, IProjectStatusRepository projectStatusRepository)
        {
            this.projectRepository = projectRepository;
            this.projectStatusRepository = projectStatusRepository;
        }

        public IQueryable<Project> GetAll()
        {
            IQueryable<Project> projects = projectRepository.GetAll();
            IQueryable<ProjectStatus> projectStatus = projectStatusRepository.GetAll();
            return from p in projects
                   from ps in projectStatus
                   where p.ProjectStatusID == ps.ProjectStatusID &&
                   ps.StatusCode != (int)ProjectStatusEnum.Uncreated
                   select p;


        }

        public Project GetById(int id)
        {
            if (id == 0)
                return null;
            return projectRepository.GetById(id);
        }

        public int Insert(Project model)
        {
            if (model == null)
                throw new ArgumentNullException("project");
            return projectRepository.Insert(model);
        }

        public void Update(Project model)
        {
            if (model == null)
                throw new ArgumentNullException("project");
            projectRepository.Update(model);

        }

        public void Delete(Project model)
        {
            if (model == null)
                throw new ArgumentNullException("project");
            projectRepository.Delete(model);
        }

        public void SaveChanges()
        {
            projectRepository.SaveChanges();
        }
    }
}