using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу сфер деятельности
    /// </summary>
    public interface IScopeRepository
    {
        IQueryable<Scope> GetAll();
        Scope GetById(int id);
        void Insert(Scope model);
    }
}
