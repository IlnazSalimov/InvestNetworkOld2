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
    /// Предоставляет метод, отвечающий за отправление сообщения пользователя
    /// </summary>
    public class MessageController : ApiController
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о пользователях
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о сообщениях пользователей
        /// </summary>
        private readonly IMessageRepository _messageRepository;

        /// <summary>
        /// Экземпляр класса InvestContext, предоставляет доступ к системным данным приложения.
        /// Может быть использован для доступа к текущему авторизованному пользователю
        /// </summary>
        private readonly IInvestContext _investContext;

        /// <summary>
        /// Инициализирует новый экземпляр ProjectCommentController с внедрением зависемостей к хранилищу данных о пользователях и их сообщениях
        /// </summary>
        /// <param name="userRepository">Экземпляр класса UserRepository, предоставляющий доступ к хранилищу данных о пользователях</param>
        /// <param name="investContext">Экземпляр класса InvestContext, предоставляющий доступ к системным данным приложения</param>
        /// <param name="messageRepository">Экземпляр класса ProjectCommentRepository, предоставляющий доступ к хранилищу данных о ссобщениях пользователей</param>
        public MessageController(IUserRepository userRepository, IInvestContext investContext, IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _investContext = investContext;
        }

        /// <summary>
        /// Отправляет сообщение пользователя
        /// </summary>
        /// <param name="model">Модель сообщения</param>
        /// <returns>Результат отправления сообщения</returns>
        [Authorize]
        [HttpPost]
        public bool Send(MessageSending model)
        {
            try
            {
                User user = _investContext.CurrentUser;

                Message msg = new Message()
                    {
                        FromUserID = user.Id,
                        MessageDate = DateTime.Now,
                        MessageText = model.Message,
                        ToUserID = model.ToUserID
                    };

                _messageRepository.Insert(msg);
                _messageRepository.SaveChanges();

                return true;
            }
            catch
            { return false; }
        }
    }
}
