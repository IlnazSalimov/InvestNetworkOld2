using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу сообщений
    /// </summary>
    public interface IMessageRepository
    {
        IQueryable<Message> GetAll();
        Message GetById(int id);
        IQueryable<Message> GetByUserId(int id);
        void Insert(Message model);
        void Update(Message model);
        void Delete(Message model);
        void SaveChanges();
    }
}
