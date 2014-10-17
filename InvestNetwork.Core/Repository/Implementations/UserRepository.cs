using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using InvestNetwork.Core;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Предоставляет методы, которые предоставляют доступ к хранилищу пользователей
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private IRepository<User> userRepository;

        public UserRepository(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }
        public IQueryable<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public User GetById(int id)
        {
            if (id == 0)
                return null;
            return userRepository.GetById(id);
        }

        public void Insert(User model)
        {
            if (model == null)
                throw new ArgumentNullException("user");
            userRepository.Insert(model);
        }

        public void Update(User model)
        {
            if (model == null)
                throw new ArgumentNullException("user");
            userRepository.Update(model);

        }

        public void Delete(User model)
        {
            if (model == null)
                throw new ArgumentNullException("user");
            userRepository.Delete(model);
        }

        public User Login(string email, string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return userRepository.GetAll()
                    .FirstOrDefault(p => string.Equals(p.Email, email) && 
                        CryptMD5.VerifyMd5Hash(md5Hash, password, p.Password));
            }
        }

        public void SaveChanges()
        {
            userRepository.SaveChanges();
        }

        public User GetByEmail(string email)
        {
            return userRepository.GetAll().FirstOrDefault(p => string.Equals(p.Email, email));
        }
    }
}

