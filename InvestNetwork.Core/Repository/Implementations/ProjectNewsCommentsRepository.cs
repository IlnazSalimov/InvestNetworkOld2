using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class ProjectNewsCommentsRepository : IProjectNewsCommentsRepository
    {
        private IRepository<ProjectNewsComment> projectNewsCommentRepository;
        private IRepository<User> userRepository;

        public ProjectNewsCommentsRepository(IRepository<ProjectNewsComment> projectNewsCommentRepository, IRepository<User> userRepository)
        {
            this.projectNewsCommentRepository = projectNewsCommentRepository;
            this.userRepository = userRepository;
        }

        public List<ProjectNewsComment> GetAll()
        {
            return projectNewsCommentRepository.GetAll().ToList();
        }

        public ProjectNewsComment GetById(int id)
        {
            if (id == 0)
                return null;
            return projectNewsCommentRepository.GetById(id);
        }

        public List<ProjectNewsComment> GetByProjectNewsId(int id)
        {
            if (id == 0)
                return null;
            var list = projectNewsCommentRepository.GetAll().Where(e => e.ProjectNewsID == id).OrderBy(e => e.CommentDate).ToList();
            foreach (ProjectNewsComment comment in list)
            {
                comment.User = userRepository.GetById(comment.FromUserID);
            }
            return list;
        }

        public void Insert(ProjectNewsComment model)
        {
            if (model == null)
                throw new ArgumentNullException("newscomment");
            projectNewsCommentRepository.Insert(model);
        }

        public void Update(ProjectNewsComment model)
        {
            if (model == null)
                throw new ArgumentNullException("newscomment");
            projectNewsCommentRepository.Update(model);

        }

        public void Delete(ProjectNewsComment model)
        {
            if (model == null)
                throw new ArgumentNullException("newscomment");
            projectNewsCommentRepository.Delete(model);
        }

        public void SaveChanges()
        {
            projectNewsCommentRepository.SaveChanges();
        }
    }
}