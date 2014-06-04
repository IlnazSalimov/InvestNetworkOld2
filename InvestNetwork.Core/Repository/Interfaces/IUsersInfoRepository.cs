using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу информации пользователя
    /// </summary>
    public interface IUsersInfoRepository
    {
        UsersInfo GetById(int id);
        UsersInfo GetByUserId(int id);
        void Insert(UsersInfo model);
        void Update(UsersInfo model);
        void Delete(UsersInfo model);
        IQueryable<PartycipationUsersInfo> GetPartycipation(int id);
        //bool ValidateUser(string email, string password);
        void SaveChanges();
    }
}
