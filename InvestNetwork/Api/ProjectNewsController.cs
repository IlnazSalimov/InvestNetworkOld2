using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InvestNetwork.Core;

namespace InvestNetwork.Api
{
    public class ProjectNewsController : ApiController
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о новостях проекта
        /// </summary>
        private readonly IProjectNewsRepository _projectNewsRepository;

        /// <summary>
        /// Инициализирует новый экземпляр ProjectNewsController с внедрением зависемостей к хранилищу данных о новостях проекта
        /// </summary>
        /// <param name="projectNewsRepository">Экземпляр класса ProjectNewsRepository, предоставляющий доступ к хранилищу данных о новостях проекта</param>
        public ProjectNewsController(IProjectNewsRepository projectNewsRepository)
        {
            _projectNewsRepository = projectNewsRepository;
        }

        /// <summary>
        /// Отправляет новость о проекте
        /// </summary>
        /// <param name="model">Модель новости</param>
        /// <returns>Модель новости, null в случае неудачи</returns>
        [Authorize]
        [HttpPost]
        public ProjectNew Send(ProjectNew _new)
        {
            try
            {
                int count = _projectNewsRepository.GetAll().Where(p => p.ProjectID == _new.ProjectID).Count();

                _new.NewsDate = DateTime.Now;
                _new.NewsTittle = "Новость №:" + (count + 1) + " " + _new.NewsTittle;
                _projectNewsRepository.Insert(_new);
                _projectNewsRepository.SaveChanges();

                return _new;
            }
            catch
            { return null; }
        }
    }
}
