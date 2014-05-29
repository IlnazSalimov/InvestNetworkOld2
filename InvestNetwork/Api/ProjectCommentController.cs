using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InvestNetwork.Core;

namespace InvestNetwork.Api
{
    /// <summary>
    /// Предоставляет метод, отвечающий за добавление комментария к проекту
    /// </summary>
    public class ProjectCommentController : ApiController
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о комментариях к проекту
        /// </summary>
        private readonly IProjectCommentRepository _projectCommentRepository;

        /// <summary>
        /// Экземпляр класса InvestContext, предоставляет доступ к системным данным приложения.
        /// Может быть использован для доступа к текущему авторизованному пользователю
        /// </summary>
        private readonly IInvestContext _investContext;
        
        /// <summary>
        /// Инициализирует новый экземпляр ProjectCommentController с внедрением зависемостей к хранилищу комметариев проектов
        /// </summary>
        /// <param name="investContext">Экземпляр класса InvestContext, предоставляющий доступ к системным данным приложения</param>
        /// <param name="projectCommentRepository">Экземпляр класса ProjectCommentRepository, предоставляющий доступ к хранилищу данных о комментариях к проекту</param>
        public ProjectCommentController(IInvestContext investContext, IProjectCommentRepository projectCommentRepository)
        {
            _projectCommentRepository = projectCommentRepository;
            _investContext = investContext;
        }

        /// <summary>
        /// Добавляет комментарий пользователя к проекту
        /// </summary>
        /// <param name="model">Модель комментария</param>
        /// <returns>Объект данных о комментарии</returns>
        [Authorize]
        [HttpPost]
        public object Send(ProjectCommentSending model)
        {
            try
            {
                User user = _investContext.CurrentUser;
                ProjectComment comment = new ProjectComment()
                    {
                        FromUserID = user.Id,
                        CommentText = model.Comment,
                        CommentDate = DateTime.Now,
                        ProjectID = model.ProjectID
                    };

                _projectCommentRepository.Insert(comment);
                _projectCommentRepository.SaveChanges();

                return new { CommentDate = comment.CommentDate.ToString("dd.MM.yyyy HH:mm:ss") };
            }
            catch
            { return false; }
        }
    }
}
