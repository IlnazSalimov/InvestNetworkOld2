using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace InvestNetwork.Core
{
    public class MessageRepository : IMessageRepository
    {
        private IRepository<Message> messageRepository;
        private IRepository<User> userRepository;

        public MessageRepository(IRepository<Message> messageRepository, IRepository<User> userRepository)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
        }
        public IQueryable<Message> GetAll()
        {
            return messageRepository.GetAll();
        }

        public Message GetById(int id)
        {
            if (id == 0)
                return null;
            return messageRepository.GetById(id);
        }

        public List<Message> GetByUserId(int id)
        {
            if (id == 0)
                return null;
            var list = messageRepository.GetAll().Where(e => e.ToUserID == id).OrderByDescending(e => e.MessageDate).ToList();
            foreach(Message message in list)
            {
                message.User = userRepository.GetById(message.FromUserID);
            }
            return list;
        }

        public void Insert(Message model)
        {
            if (model == null)
                throw new ArgumentNullException("message");
            messageRepository.Insert(model);
        }

        public void Update(Message model)
        {
            if (model == null)
                throw new ArgumentNullException("message");
            messageRepository.Update(model);

        }

        public void Delete(Message model)
        {
            if (model == null)
                throw new ArgumentNullException("message");
            messageRepository.Delete(model);
        }

        public void SaveChanges()
        {
            messageRepository.SaveChanges();
        }
    }
}

