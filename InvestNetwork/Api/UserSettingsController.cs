using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using InvestNetwork.Core;

namespace InvestNetwork.Api
{
    /// <summary>
    /// Предоставляет метод, отвечающий за редактирование настроек профиля пользователя
    /// </summary>
    public class UserSettingsController : ApiController
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о пользователях
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Экземпляр класса InvestContext, предоставляет доступ к системным данным приложения.
        /// Может быть использован для доступа к текущему авторизованному пользователю
        /// </summary>
        private readonly IInvestContext _investContext;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера настроек профиля пользователя
        /// </summary>
        /// <param name="userRepository">Экземпляр класса UserRepository, предоставляющий доступ к хранилищу данных о пользователях</param>
        /// <param name="investContext">Экземпляр класса InvestContext, предоставляющий доступ к системным данным приложения</param>
        public UserSettingsController(IUserRepository userRepository, IInvestContext investContext)
        {
            _userRepository = userRepository;
            _investContext = investContext;
        }

        /// <summary>
        /// Редактирует личную информацию пользователя
        /// </summary>
        /// <param name="model">Модель настроек профиля пользователя</param>
        /// <returns>Объект результата сохранения профиля пользователя</returns>
        [Authorize]
        [HttpPost]
        public object Edit(UserSettings model)
        {
            User user = _investContext.CurrentUser;

            if (model.NewPassword != null)
            {

                if (!model.OldPassword.Equals(user.Password))
                {
                    return new { isSuccess = false, errorMessage = "Действующий пароль введен неверно", successMessage = "" };
                }

                if (!model.NewPassword.Equals(model.ConfirmPassword))
                {
                    return new { isSuccess = false, errorMessage = "Подтвердите новый пароль", successMessage = "" };
                }

                user.Password = model.NewPassword.ToString();
            }

            if (model.Email == null)
            {
                model.Email = user.Email;
            }
            else
            {
                if (!model.Email.Equals(user.Email))
                {
                    var anyUser = _userRepository.GetAll().Any(p => p.Email.Equals(model.Email));
                    if (anyUser)
                    {
                        return new { isSuccess = false, errorMessage = "Пользователь с таким email уже зарегистрирован", successMessage = "" };
                    }

                    Regex rgx = new Regex("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$");
                    if (!rgx.IsMatch(model.Email))
                    {
                        return new { isSuccess = false, errorMessage = "Введен некорректный email", successMessage = "" };
                    }
                    user.Email = model.Email;
                }
            }

            if (model.FullName == null)
            {
                model.FullName = user.FullName;
            }
            else
            {
                if (!model.FullName.Equals(user.FullName))
                {
                    var anyUser = _userRepository.GetAll().Any(p => p.FullName.Equals(model.FullName));
                    if (anyUser)
                    {
                        return new { isSuccess = false, errorMessage = "Пользователь с таким именем уже зарегистрирован", successMessage = "" };
                    }
                }
                user.FullName = model.FullName;
            }

            if (model.Avatar == null)
            {
                model.Avatar = user.Avatar;
            }

            _userRepository.Update(user);

            _userRepository.SaveChanges();

            return new { isSuccess = true, errorMessage = "", successMessage = "Данные успешно сохранены" };
        }
    }
}
