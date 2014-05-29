using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text.RegularExpressions;
using Calabonga.Mvc.Extensions;
using Calabonga.Xml.Exports;
using InvestNetwork.Core;

namespace InvestNetwork.Controllers
{
    /// <summary>
    /// Предоставляет методы, которые отвечают за бизнес логику аутентификации
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о пользователях
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о ролях пользователей
        /// </summary>
        private readonly IRoleRepository _roleRepository;

        /// <summary>
        /// Экземпляр класса InvestContext, предоставляет доступ к системным данным приложения.
        /// Может быть использован для доступа к текущему авторизованному пользователю
        /// </summary>
        private readonly IInvestContext _investContext;

        /// <summary>
        /// Инициализирует новый экземпляр AccountController с внедрением зависемостей к хранилищу данных о пользователях и их сообщениях
        /// </summary>
        /// <param name="userRepository">Экземпляр класса UserRepository, предоставляющий доступ к хранилищу данных о пользователях</param>
        /// <param name="roleRepository">Экземпляр класса RoleRepository, предоставляющий доступ к хранилищу данных о ролях пользователей</param>
        /// <param name="investContext">Экземпляр класса InvestContext, предоставляющий доступ к системным данным приложения</param>
        public AccountController(IUserRepository userRepository, IRoleRepository roleRepository, IInvestContext investContext)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._investContext = investContext;
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на главной странице аутентификации.
        /// </summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице регистрации для get-запроса.</summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице регистрации для post-запроса.</summary>
        /// <param name="model">Модель регистрации</param>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        [HttpPost]
        //[Captcher(MessageText = "Неверный код с картинки")]
        public ActionResult SignUp(SignupUser model)
        {
            if (ModelState.IsValid)
            {
                var anyUser = _userRepository.GetAll().Any(p => p.Email.Equals(model.Email));
                if (anyUser)
                {
                    return View();
                }

                Regex rgx = new Regex("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$");
                if (!rgx.IsMatch(model.Email))
                {
                    return View();
                }

                anyUser = _userRepository.GetAll().Any(p => p.FullName.Equals(model.FullName));
                if (anyUser)
                {
                    return View();
                }

                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    return View();
                }

                _userRepository.Insert(new User { FullName = model.FullName, Email = model.Email, Password = model.Password });
                _userRepository.SaveChanges();
                
                LoginUser login = new LoginUser()
                {
                    Email = model.Email,
                    Password = model.Password,
                    RememberMe = true
                };
                Login(login, "");
            }
            return View();
        }

        /*public ActionResult SetAdminRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                User currentUser = _userRepository.GetAll().SingleOrDefault(user => string.Equals(user.Email, User.Identity.Name));
                currentUser.Roles.Add(_roleRepository.GetAll().SingleOrDefault(role => string.Equals(role.RoleName, "Admin")));
                _userRepository.Update(currentUser);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }*/

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице авторизации для get-запроса.</summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице авторизации для post-запроса.</summary>
        /// <param name="model">Модель авторизации</param>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        [HttpPost]
        public ActionResult Login(LoginUser model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _investContext.Auth.Login(model.Email, model.Password, model.RememberMe);

                if (user != null)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Имя пользователя или пароль является не корректным.");
                }
            }

            return View(model);
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице выхода из профиля.</summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        public ActionResult Logout()
        {
            _investContext.Auth.LogOut();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Метод отвечающий за вывод captcha.</summary>
        /// </summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления</returns>
        /*public ActionResult Captcha()
        {
            return new CaptchaResult();
        }*/
    }
}
