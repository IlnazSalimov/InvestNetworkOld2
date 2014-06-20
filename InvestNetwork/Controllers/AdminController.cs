using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using System.Threading.Tasks;

namespace InvestNetwork.Controllers
{
    /// <summary>
    /// Предоставляет методы, которые отвечают за бизнес логику административной части </summary>
    /// <remarks>Предоставляет доступ только пользователям с ролью Admin</remarks>
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        /// <summary>
        /// Количество проектов, которые будут показываться в блоке "Последние проекты"</summary>
        private const int RECENT_PROJECT_CNT = 3;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о проектах.</summary>
        private readonly IProjectRepository _projectRepository;

        /// <summary>  
        /// Инициализирует новый экземпляр AdminController с внедрением зависемостей к хранилищу проектов.</summary>  
        /// <param name="projectRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о проектах.</param>
        /// <param name="projectStatusRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о статусах проектах</param>
        /// <returns>Новый экземпляр AdminController.</returns>
        public AdminController(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на главной странице административной части.</summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        public ActionResult Index()
        {
            ViewBag.Projects = _projectRepository.GetAll().OrderBy(p => p.EndDate).Take(20);
            ViewBag.RecentProjects = _projectRepository.GetAll().OrderByDescending(p => p.CreateDate).Take(RECENT_PROJECT_CNT);
            return View();
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице проверки проекта с заданным идентификатором.</summary>
        /// <param name="Id">Идентификатор проекта</param>
        /// <returns>Экземпляр ViewResult с моделью проекта, который выполняет визуализацию представления.</returns>
        public async Task<ActionResult> ReviewProject(int Id)
        {
            return View(await _projectRepository.GetByIdAsync(Id));
        }


        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице всех проектов находящихся на проверке.</summary>
        /// <param name="Id">Идентификатор проекта</param>
        /// <returns>Экземпляр ViewResult со списком проектов, который выполняет визуализацию представления.</returns>
        public ActionResult GetReviewingProjects()
        {
            return View(_projectRepository.GetAll().Where(p => p.Status == ProjectStatusEnum.OnReview));
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице со списком неинспектированных проектов.</summary>
        /// <param name="Id">Идентификатор проекта</param>
        /// <returns>Экземпляр ViewResult со списком неинспектированных проектов, который выполняет визуализацию представления.</returns>
        public ActionResult GetInspectingProjects()
        {
            return View(_projectRepository.GetAll().Where(p => !p.IsInspected));
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику просмотра всех проектов.</summary>
        /// <param name="Id">Идентификатор проекта</param>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        public ActionResult GetAllProjects()
        {
            return View();
        }
    }
}
