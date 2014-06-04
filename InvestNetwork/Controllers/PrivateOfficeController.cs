using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using InvestNetwork.Core;

namespace InvestNetwork.Controllers
{
    /// <summary>
    /// Предоставляет методы, которые отвечают за бизнес логику личного кабинета пользователя
    /// </summary>
    public class PrivateOfficeController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу личных данных пользователей
        /// </summary>
        private readonly IUsersInfoRepository _usersInfoRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о пользователях
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о сообщениях пользователей
        /// </summary>
        private readonly IMessageRepository _messageRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о проектах
        /// </summary>
        private readonly IProjectRepository _projectRepository;

        /// <summary>
        /// Экземпляр класса InvestContext, предоставляет доступ к системным данным приложения.
        /// Может быть использован для доступа к текущему авторизованному пользователю
        /// </summary>
        private readonly IInvestContext _investContext;

        /// <summary>
        /// Инициализирует новый экземпляр PrivateOfficeController с внедрением зависемостей к хранилищу данных о пользователях, их личной информации, настройках, проектах исообщениях
        /// </summary>
        /// <param name="userRepository">Экземпляр класса UserRepository, предоставляющий доступ к хранилищу данных о пользователях</param>
        /// <param name="usersInfoRepository">Экземпляр класса UsersInfoRepository, предоставляющий доступ к хранилищу личных данных о пользователях</param>
        /// <param name="messageRepository">Экземпляр класса MessageRepository, предоставляющий доступ к хранилищу данных сообщених пользователей</param>
        /// <param name="projectRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о проектах</param>
        /// <param name="investContext">Экземпляр класса InvestContext, предоставляющий доступ к системным данным приложения</param>
        public PrivateOfficeController(IUserRepository userRepository, IUsersInfoRepository usersInfoRepository, 
                                       IMessageRepository messageRepository, IProjectRepository projectRepository,
                                       IInvestContext investContext)
        {
            _userRepository = DependencyResolver.Current.GetService<IUserRepository>();
            _usersInfoRepository = usersInfoRepository;
            _messageRepository = messageRepository;
            _projectRepository = projectRepository;
            _investContext = investContext;
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице личного кабинета пользователя.
        /// </summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            User user = _investContext.CurrentUser;

            ViewBag.usersInfo = _usersInfoRepository.GetByUserId(user.Id);
            ViewBag.messages = _messageRepository.GetByUserId(user.Id);
            ViewBag.partycipations = _usersInfoRepository.GetPartycipation(user.ID);
            ViewBag.projects = _projectRepository.GetAll().Where(e => e.AuthorID == user.Id).ToList();
            UserSettings userSettings = new UserSettings();
            
            userSettings.Avatar = user.Avatar;
            userSettings.Email = user.Email;
            userSettings.FullName = user.FullName;
            userSettings.Id = user.Id;
            userSettings.PostNotice = user.PostNotice;
            userSettings.RoleId = user.RoleId;

            ViewBag.userSettings = userSettings;

            return View(user);
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на главной странице аутентификации.
        /// </summary>
        /// <param name="file">Объект, представляющий собой изображение профиля пользователя</param>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        [Authorize]
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            User user = _investContext.CurrentUser;

            ViewBag.usersInfo = _usersInfoRepository.GetByUserId(user.Id);
            ViewBag.messages = _messageRepository.GetByUserId(user.Id);
            ViewBag.partycipations = _usersInfoRepository.GetPartycipation(user.ID);
            ViewBag.projects = _projectRepository.GetAll().Where(e => e.AuthorID == user.Id).ToList();
            UserSettings userSettings = new UserSettings();

            userSettings.Avatar = user.Avatar;
            userSettings.Email = user.Email;
            userSettings.FullName = user.FullName;
            userSettings.Id = user.Id;
            userSettings.PostNotice = user.PostNotice;
            userSettings.RoleId = user.RoleId;

            ViewBag.userSettings = userSettings;
            try
            {
                if ((file != null && file.ContentLength > 0))
                {
                    string relativePathOfDir = Path.Combine(
                        ConfigurationManager.AppSettings["FileUploadDirectory"].ToString(),
                        "user" + user.ID.ToString());
                    string FullPathOfDir = Server.MapPath(relativePathOfDir);
                    if (!Directory.Exists(FullPathOfDir))
                    {
                        Directory.CreateDirectory(FullPathOfDir);
                    }
                    string savedFilePath = Path.Combine(FullPathOfDir, file.FileName);
                    string relativeFilePath = Path.Combine(relativePathOfDir, file.FileName);
                    if (System.IO.File.Exists(savedFilePath))
                    {
                        System.IO.File.Delete(savedFilePath);
                    }
                    file.SaveAs(savedFilePath);

                    user.Avatar = relativeFilePath;
                    userSettings.Avatar = user.Avatar;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                }
                else
                {
                    return View(user);
                }
            }
            catch
            {
                return View(user);
            }
            return View(user);
        }
    }
}
