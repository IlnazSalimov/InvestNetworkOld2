using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace InvestNetwork.Core
{
    public class ProjectCommentRepository : IProjectCommentRepository
    {
        private IRepository<ProjectComment> projectCommentRepository;
        private IRepository<User> userRepository;

        public ProjectCommentRepository(IRepository<ProjectComment> projectCommentRepository, IRepository<User> userRepository)
        {
            this.projectCommentRepository = projectCommentRepository;
            this.userRepository = userRepository;
        }
        public IQueryable<ProjectComment> GetAll()
        {
            return projectCommentRepository.GetAll();
        }

        public ProjectComment GetById(int id)
        {
            if (id == 0)
                return null;
            return projectCommentRepository.GetById(id);
        }

        public List<ProjectComment> GetByProjectId(int id)
        {
            if (id == 0)
                return null;
            var list = projectCommentRepository.GetAll().Where(e => e.ProjectID == id).OrderByDescending(e => e.CommentDate).ToList();
            foreach (ProjectComment comment in list)
            {
                comment.User = userRepository.GetById(comment.FromUserID);
            }
            return list;
        }

        public void Insert(ProjectComment model)
        {
            if (model == null)
                throw new ArgumentNullException("message");
            projectCommentRepository.Insert(model);
        }

        public void Update(ProjectComment model)
        {
            if (model == null)
                throw new ArgumentNullException("message");
            projectCommentRepository.Update(model);

        }

        public void Delete(ProjectComment model)
        {
            if (model == null)
                throw new ArgumentNullException("message");
            projectCommentRepository.Delete(model);
        }

        public void SaveChanges()
        {
            projectCommentRepository.SaveChanges();
        }
    }
}

