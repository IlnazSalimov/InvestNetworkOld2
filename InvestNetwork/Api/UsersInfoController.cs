using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InvestNetwork.Core;

namespace InvestNetwork.Api
{
    /// <summary>
    /// Предоставляет метод, отвечающий за редактирование личной информации пользователя
    /// </summary>
    public class UsersInfoController : ApiController
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных личной информации пользователя
        /// </summary>
        private readonly IUsersInfoRepository _usersInfoRepository;

        /// <summary>
        /// Экземпляр класса InvestContext, предоставляет доступ к системным данным приложения.
        /// Может быть использован для доступа к текущему авторизованному пользователю
        /// </summary>
        private readonly IInvestContext _investContext;

        /// <summary>
        /// Инициализирует новый экземпляр UsersInfoController с внедрением зависемостей к хранилищу личной информации пользователей
        /// </summary>
        /// <param name="usersInfoRepository">Экземпляр класса ProjectCommentRepository, предоставляющий доступ к хранилищу данных о пользователях</param>
        /// <param name="investContext">Экземпляр класса InvestContext, предоставляющий доступ к системным данным приложения</param>
        public UsersInfoController(IUsersInfoRepository usersInfoRepository, IInvestContext investContext)
        {
            _usersInfoRepository = usersInfoRepository;
            _investContext = investContext;
        }

        /// <summary>
        /// Редактирует личную информацию пользователя
        /// </summary>
        /// <param name="model">Модель личной информации пользователя</param>
        /// <returns>Результат сохранения личной информации</returns>
        [Authorize]
        [HttpPost]
        public object Edit(UsersInfo model)
        {
            if (ModelState.IsValid)
            {
                User user = _investContext.CurrentUser;

                model.UserID = user.Id;
                UsersInfo usersInfo = _usersInfoRepository.GetByUserId(user.Id);

                if (usersInfo == null)
                {
                    model.RegisterDate = DateTime.Now;
                    _usersInfoRepository.Insert(model);
                }
                else
                {
                    model.UsersInfoID = usersInfo.UsersInfoID;
                    model.RegisterDate = usersInfo.RegisterDate;
                    _usersInfoRepository.Update(model);
                }

                _usersInfoRepository.SaveChanges();

            }
            else
            {
                return new { isSuccess = false, errorMessage = "Данные введены некорректно", successMessage = "" }; // Может быть создать отделный класс ответов и ошибки описывать подробнее до полей где произошла ошибка
            }
            return new {isSuccess = true, errorMessage = "", successMessage = "Данные успешно сохранены" };
        }
    }
}
