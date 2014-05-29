using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InvestNetwork.Core;

namespace InvestNetwork.Api
{
    public class ProjectNewsCommentController : ApiController
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о комментариях к новости проекта
        /// </summary>
        private readonly IProjectNewsCommentsRepository _projectNewsCommentsRepository;

        /// <summary>
        /// Экземпляр класса InvestContext, предоставляет доступ к системным данным приложения.
        /// Может быть использован для доступа к текущему авторизованному пользователю
        /// </summary>
        private readonly IInvestContext _investContext;
        
        /// <summary>
        /// Инициализирует новый экземпляр ProjectNewsCommentController с внедрением зависемостей к хранилищу комметариев новостей проектов
        /// </summary>
        /// <param name="investContext">Экземпляр класса InvestContext, предоставляющий доступ к системным данным приложения</param>
        /// <param name="projectNewsCommentsRepository">Экземпляр класса ProjectNewsCommentsRepository, предоставляющий доступ к хранилищу данных о комментариях к новости проекта</param>
        public ProjectNewsCommentController(IInvestContext investContext, IProjectNewsCommentsRepository projectNewsCommentsRepository)
        {
            _projectNewsCommentsRepository = projectNewsCommentsRepository;
            _investContext = investContext;
        }

        /// <summary>
        /// Добавляет комментарий пользователя к проекту
        /// </summary>
        /// <param name="model">Объект комментария</param>
        /// <returns>Модель комментария, null в случае неудачи</returns>
        [Authorize]
        [HttpPost]
        public object Send(ProjectNewsComment model)
        {
            try
            {
                User user = _investContext.CurrentUser;
                model.FromUserID = user.Id;
                model.CommentDate = DateTime.Now;

                _projectNewsCommentsRepository.Insert(model);
                _projectNewsCommentsRepository.SaveChanges();

                return new { CommentDate = model.CommentDate.ToString("dd.MM.yyyy HH:mm:ss") };
            }
            catch
            { return null; }
        }
    }
}
