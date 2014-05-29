using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using InvestNetwork.Application.Core;

namespace InvestNetwork.Api
{
    /// <summary>
    /// Предоставляет api методы, которые отвечают за бизнес логику административной части </summary>
    /// <remarks>Предоставляет доступ только пользователям с ролью Admin</remarks>
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiController
    {
        /// <summary>
        /// Преобразователь одной модели в другую.</summary>
        private IMapper _modelMapper { get; set; }

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о проектах.</summary>
        private readonly IProjectRepository _projectRepository;

        /// <summary>  
        /// Инициализирует новый экземпляр AdminController с внедрением зависемостей к хранилищу проектов.</summary>  
        /// <param name="projectRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о проектах.</param>
        /// <param name="projectStatusRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о статусах проектах</param>
        /// <returns>Новый экземпляр AdminController.</returns>
        public AdminController(IProjectRepository projectRepository, IMapper mapper)
        {
             this._modelMapper = mapper;
            this._projectRepository = projectRepository;
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику блокировки проекта с заданным идентификатором.</summary>
        /// <param name="id">Идентификатор проекта</param>
        /// <returns>Экземпляр HttpResponseMessage с результатом.</returns>
        [HttpGet]
        public HttpResponseMessage BlockProject(int id)
        {
            Project reviewingProject = _projectRepository.GetById(id);
            if (reviewingProject == null)
            {
                var message = string.Format("Проект с id = {0} не найден", id);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }

            reviewingProject.Status = ProjectStatusEnum.Blocked;
            reviewingProject.IsInspected = true;
             
            try
            {
                _projectRepository.SaveChanges();
            }
            catch (HttpResponseException ex)
            {
                var message = string.Format("Во время сохранении проекта с id = {0} произошла ошибка", id);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
            /*HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Moved);
            string relativeUrl = Url.Route("Default", new { controller = "Admin", action = "ReviewProject", id = id });
            response.Headers.Location = new Uri(Url.Absolute(relativeUrl));
            throw new HttpResponseException(response);*/

            return Request.CreateResponse(HttpStatusCode.OK, _modelMapper.Map(reviewingProject, typeof(Project), typeof(ProjectDTO)) as ProjectDTO);
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику публикации проекта с заданным идентификатором.</summary>
        /// <param name="id">Идентификатор проекта</param>
        /// <returns>Экземпляр RedirectToRouteResult с названием метода действия и идентификатором проекта, который выполняет перенаправление к заданному методу.</returns>
        [HttpGet]
        public HttpResponseMessage PublishProject(int id)
        {
            Project reviewingProject = _projectRepository.GetById(id);
            if (reviewingProject == null)
            {
                var message = string.Format("Проект с id = {0} не найден", id);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }

            reviewingProject.Status = ProjectStatusEnum.Blocked;
            reviewingProject.IsInspected = true;

            try
            {
                _projectRepository.SaveChanges();
            }
            catch (HttpResponseException ex)
            {
                var message = string.Format("Во время сохранении проекта с id = {0} произошла ошибка", id);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
            /*HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Moved);
            string relativeUrl = Url.Route("Default", new { controller = "Admin", action = "ReviewProject", id = id });
            response.Headers.Location = new Uri(Url.Absolute(relativeUrl));
            throw new HttpResponseException(response);*/

            return Request.CreateResponse(HttpStatusCode.OK, _modelMapper.Map(reviewingProject, typeof(Project), typeof(ProjectDTO)) as ProjectDTO);
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику публикации проекта с заданным идентификатором.</summary>
        /// <param name="Id">Идентификатор проекта</param>
        /// <returns>Экземпляр RedirectToRouteResult с названием метода действия и идентификатором проекта, который выполняет перенаправление к заданному методу.</returns>
        /*public ActionResult PublishProject(int Id)
        {
            Project reviewingProject = _projectRepository.GetById(Id);
            reviewingProject.Status = ProjectStatusEnum.Active;
            reviewingProject.IsInspected = true;
            _projectRepository.SaveChanges();
            return RedirectToAction("ReviewProject", new { Id = Id });
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику одобрения проекта с заданным идентификатором.</summary>
        /// <param name="Id">Идентификатор проекта</param>
        /// <returns>Экземпляр RedirectToRouteResult с названием метода действия и идентификатором проекта, который выполняет перенаправление к заданному методу.</returns>
        public ActionResult ApproveProject(int Id)
        {
            Project reviewingProject = _projectRepository.GetById(Id);
            reviewingProject.IsInspected = true;
            _projectRepository.SaveChanges();
            return RedirectToAction("ReviewProject", new { Id = Id });
        }*/
    }
}
