using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using InvestNetwork.Application.Core;
using System.Threading.Tasks;

namespace InvestNetwork.Api
{
    /// <summary>
    /// Предоставляет методы, которые отвечают за бизнес логику административной части </summary>
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
        public AdminController(IProjectRepository projectRepository, IMapper mapper)
        {
             this._modelMapper = mapper;
            this._projectRepository = projectRepository;
        }

        private ProjectStatusEnum ConvertStatusToEnum(string status)
        {
            switch (status.ToLower())
            {
                case "active": return ProjectStatusEnum.Active; break;
                case "blocked": return ProjectStatusEnum.Blocked; break;
                case "inactive": return ProjectStatusEnum.Inactive; break;
                case "onreview": return ProjectStatusEnum.OnReview; break;
                case "uncreated": return ProjectStatusEnum.Uncreated; break;
                default: return 0;
            }
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику изменения статуса проекта с заданным идентификатором.</summary>
        /// <param name="id">Идентификатор проекта</param>
        /// <param name="status">Устанавливаемы статус проекта</param>
        /// <returns>Экземпляр HttpResponseMessage с результатом.</returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SetProjectStatus(int id, string status)
        {
            Project reviewingProject = await _projectRepository.GetByIdAsync(id);
            if (reviewingProject == null)
            {
                var message = string.Format("Проект с id = {0} не найден", id);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }

            ProjectStatusEnum projectStatus = ConvertStatusToEnum(status);

            if (projectStatus == 0)
            {
                var message = string.Format("Статуса \"{0}\" не существует", status);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }

            reviewingProject.Status = projectStatus;
            reviewingProject.IsInspected = true;
             
            try
            {
                await _projectRepository.EditAsync(reviewingProject);
            }
            catch (HttpResponseException ex)
            {
                var message = string.Format("Во время сохранении проекта с id = {0} произошла ошибка", id);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Moved);
            string relativeUrl = Url.Route("Default", new { controller = "Admin", action = "GetInspectingProjects"});
            response.Headers.Location = new Uri(Url.Absolute(relativeUrl));
            throw new HttpResponseException(response);
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику инспекции проекта с заданным идентификатором.</summary>
        /// <param name="id">Идентификатор проекта</param>
        /// <returns>Экземпляр HttpResponseMessage с результатом.</returns>
        [HttpGet]
        public async Task<HttpResponseMessage> Inpect(int id)
        {
            Project reviewingProject = await _projectRepository.GetByIdAsync(id);
            if (reviewingProject == null)
            {
                var message = string.Format("Проект с id = {0} не найден", id);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }

            reviewingProject.IsInspected = true;

            try
            {
                await _projectRepository.EditAsync(reviewingProject);
            }
            catch (HttpResponseException ex)
            {
                var message = string.Format("Во время сохранении проекта с id = {0} произошла ошибка", id);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Moved);
            string relativeUrl = Url.Route("Default", new { controller = "Admin", action = "ReviewProject", id = id });
            response.Headers.Location = new Uri(Url.Absolute(relativeUrl));
            throw new HttpResponseException(response);
        }

    }
}
