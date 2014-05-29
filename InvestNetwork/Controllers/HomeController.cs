
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InvestNetwork.Core;

namespace InvestNetwork.Controllers
{
    /// <summary>
    /// Предоставляет методы, которые отвечают за бизнес логику главной страницы сайта 
    /// </summary>
    public class HomeController : Controller//, IGeneralController
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о пользователях
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о городах
        /// </summary>
        private readonly ICityRepository _cityRepository;

        /// <summary>
        /// Инициализирует новый экземпляр HomeController с внедрением зависемостей к хранилищу данных о пользователях и городах
        /// </summary>
        /// <param name="userRepository">Экземпляр класса UserRepository, предоставляющий доступ к хранилищу данных о пользователях</param>
        /// <param name="cityRepository">Экземпляр класса CityRepository, предоставляющий доступ к хранилищу данных о городах</param>
        public HomeController(IUserRepository userRepository, ICityRepository cityRepository)
        {
            _userRepository = userRepository;
            _cityRepository = cityRepository;
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на главной странице сайта.
        /// </summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
