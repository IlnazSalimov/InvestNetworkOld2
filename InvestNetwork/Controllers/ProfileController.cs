using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InvestNetwork.Controllers
{
    /// <summary>
    /// Предоставляет методы, которые отвечают за бизнес логику профиля пользователя
    /// </summary>
    public class ProfileController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о пользователях
        /// </summary>
        readonly private IUserRepository userRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу личных данных пользователей
        /// </summary>
        readonly private IUsersInfoRepository usersInfoRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о проектах
        /// </summary>
        readonly private IProjectRepository projectRepository;

        /// <summary>
        /// Инициализирует новый экземпляр ProfileController с внедрением зависемостей к хранилищу данных о пользователях, их личной информации и проектах
        /// </summary>
        /// <param name="userRepository">Экземпляр класса UserRepository, предоставляющий доступ к хранилищу данных о пользователях</param>
        /// <param name="usersInfoRepository">Экземпляр класса UserRepository, предоставляющий доступ к хранилищу личных данных пользователей</param>
        /// <param name="projectRepository">Экземпляр класса UserRepository, предоставляющий доступ к хранилищу данных о проектах</param>
        public ProfileController(IUserRepository userRepository, IUsersInfoRepository usersInfoRepository, IProjectRepository projectRepository)
        {
            this.userRepository = userRepository;
            this.usersInfoRepository = usersInfoRepository;
            this.projectRepository = projectRepository;
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице регистрации для get-запроса.</summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        [HttpGet]
        public ActionResult GetProfile(int id)
        {
            UserProfile profile = new UserProfile()
            {
                User = userRepository.GetById(id),
                UsersInfo = usersInfoRepository.GetByUserId(id),
                Projects = projectRepository.GetAll().Where(e => e.AuthorID == id).ToList()
            };

            return View(profile);
        }

    }
}
