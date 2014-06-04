using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace InvestNetwork.Controllers
{
    /// <summary>
    /// Предоставляет методы, которые отвечают за бизнес логику управления проектами</summary>
    public class ProjectController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о проектах.</summary>
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectNewsCommentsRepository _projectNewsCommentsRepository;
        /// Предоставляет доступ к хранилищу данных о новостях проекта.</summary>
        private readonly IProjectNewsRepository _projectNewsRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о комментариях проекта.</summary>
        private readonly IProjectCommentRepository _projectCommentRepository;

        /// <summary>
        /// Экземпляр класса InvestContext, предоставляет доступ к системным данным приложения.
        /// Может быть использован для доступа к текущему авторизованному пользователю.
        /// </summary>
        private readonly IInvestContext _investContext;

        /// <summary>
        /// Количество проектов отображаемых при первом просмотре страницы с проектами
        /// </summary>
        private const int PROJECT_COUNT_AT_THE_FIRST_VIEWING = 20;

        /// <summary>  
        /// Инициализирует новый экземпляр ProjectController с внедрением зависемостей к хранилищам проектов,
        /// новостей проекта, комментариев проекта и систымных данных приложения.</summary>  
        /// <param name="projectRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о проектах.</param>
        /// <param name="projectNewsRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о новостях проекта.</param>
        /// <param name="projectCommentRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о новостях проекта.</param>
        /// <param name="investContext">Экземпляр класса InvestContext, предоставляет доступ к системным данным приложения.</param>
        /// <returns>Новый экземпляр ProjectController.</returns>
        public ProjectController(IProjectRepository projectRepository, IProjectNewsRepository projectNewsRepository,
            IProjectCommentRepository projectCommentRepository, IInvestContext investContext, IProjectNewsCommentsRepository projectNewsCommentsRepository)
        {
            this._projectRepository = projectRepository;
            this._projectNewsRepository = projectNewsRepository;
            this._projectNewsCommentsRepository = projectNewsCommentsRepository;
            this._projectCommentRepository = projectCommentRepository;
            this._investContext = investContext;
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на главной странице просмотра проектов.</summary>
        /// <returns>Экземпляр ViewResult со списком проектов, который выполняет визуализацию представления.</returns>
        public ActionResult Index()
        {
            return View(_projectRepository.GetAll().Take(PROJECT_COUNT_AT_THE_FIRST_VIEWING));
        }


        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице первого шага регестрации проекта.</summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        /// <remarks>Метод используется для обработки GET запросов. Предоставляет доступ только авторизованным пользователям</remarks>
        [Authorize]
        public ActionResult Start()
        {
            return View();
        }


        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице первого шага регестрации проекта.</summary>
        /// <param name="model">Модель проекта, который необходимо сохранить в базе данных.</param>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        /// <remarks>Метод используется для обработки POST запросов. Предоставляет доступ только авторизованным пользователям</remarks>
        [Authorize]
        [HttpPost]
        public ActionResult Start(ProjectStart model)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project
                {
                    AuthorID = _investContext.CurrentUser.Id,
                    CreateDate = DateTime.Now,
                    Status = ProjectStatusEnum.Uncreated,
                    IsInspected = false,
                    ProjectFilesDirectory = "",
                    Name = model.Name,
                    FundingDuration = model.FundingDuration,
                    LocationCityID = model.LocationCityID,
                    NecessaryFunding = model.NecessaryFunding,
                    ScopeID = model.ScopeID,
                    ShortDescription = model.ShortDescription
                };

                try
                {
                    project.ProjectID = _projectRepository.Insert(project);
                    _projectRepository.SaveChanges();

                    string projectFilesDirectoryName = String.Format("project{0}in{1}", project.ID.ToString(), project.CreateDate.ToString("dd_MM_yyyy"));
                    string userDirRelPath = Path.Combine(ConfigurationManager.AppSettings["FileUploadDirectory"].ToString(), _investContext.CurrentUser.FilesBrowserDirectory);
                    string projectDirRelPath = Path.Combine(userDirRelPath, projectFilesDirectoryName);
                    
                    Directory.CreateDirectory(Server.MapPath(projectDirRelPath));
                    project.ProjectFilesDirectory = projectFilesDirectoryName;
                    
                    _projectRepository.SaveChanges();
                }
                catch (Exception ex) { }

                return RedirectToAction("CompleteSecondStepOfStart", new { Id = project.ID });
            }
            else
            {
                return View(model);
            }

            
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице второго шага регестрации проекта.</summary>
        /// <param name="Id">Идентификатор проекта.</param>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        /// <remarks>Метод используется для обработки GET запросов. Предоставляет доступ только авторизованным пользователям</remarks>
        [Authorize]
        public ActionResult CompleteSecondStepOfStart(int Id)
        {
            try
            {
                Project fillingProject = _projectRepository.GetById(Id);
                return View(new ProjectStartingSecondStep { 
                    ProjectID = fillingProject.ID,
                    ProjectFilesDirectory = fillingProject.ProjectFilesDirectory 
                });
            }
            catch (Exception ex) { }

            return Start();
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице второго шага регестрации проекта.</summary>
        /// <param name="model">Модель проекта, который необходимо сохранить в базе данных.</param>
        /// <param name="ProjectImg">Объект изображения проекта.</param>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        /// <remarks>Метод используется для обработки POST запросов. Предоставляет доступ только авторизованным пользователям</remarks>
        [Authorize]
        [HttpPost]
        public ActionResult CompleteSecondStepOfStart(ProjectStartingSecondStep model, HttpPostedFileBase LinkToImg)
        {
            if (ModelState.IsValid && (LinkToImg != null && LinkToImg.ContentLength > 0))
            {
                Project fillingProject = _projectRepository.GetById(model.ProjectID);
                fillingProject.Description = model.Description;
                fillingProject.Status = ProjectStatusEnum.Active;
                fillingProject.StartDate = DateTime.Now;
                fillingProject.EndDate = fillingProject.StartDate.Value.AddDays((int)fillingProject.FundingDuration.Value);
                fillingProject.LinkToImg = FileUploader.Upload(LinkToImg, fillingProject.ProjectFilesDirectory);
                
                _projectRepository.SaveChanges();

                return RedirectToAction("Discover");
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице просмотра проектов.</summary>
        /// <returns>Экземпляр ViewResult со списком проектов, который выполняет визуализацию представления.</returns>
        public ActionResult Discover()
        {
            IProjectStatusRepository psr = DependencyResolver.Current.GetService<IProjectStatusRepository>();
            IQueryable<ProjectStatus> projectStatus = psr.GetAll();
            var q = (from p in _projectRepository.GetAll()
                     from ps in projectStatus
                    where p.ProjectStatusID == ps.ProjectStatusID &&
                    ps.StatusCode == (int)ProjectStatusEnum.Active
                    select p).Take(PROJECT_COUNT_AT_THE_FIRST_VIEWING);
            return View(q.ToList());
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице просмотра проекта с заданным идентификатором.</summary>
        /// <param name="Id">Идентификатор проекта.</param>
        /// <returns>Экземпляр ViewResult с моделью проекта, который выполняет визуализацию представления.</returns>
        public ActionResult View(int id)
        {
            List<ProjectNew> projectNews = _projectNewsRepository.GetAll().Where(p => p.ProjectID == id).OrderByDescending(p => p.NewsDate).ToList();
            foreach (ProjectNew _new in projectNews)
            {
                _new.ProjectNewsComments = _projectNewsCommentsRepository.GetByProjectNewsId(_new.ProjectNewsID);
            }
            ViewBag.projectNews = projectNews; 
            ViewBag.projectComments = _projectCommentRepository.GetByProjectId(id);
            ViewBag.user = _investContext.CurrentUser;
            ViewBag.currDate = DateTime.Now;
            return View(_projectRepository.GetById(id));
        }
    }
}
