using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу стран
    /// </summary>
    public interface ICountryRepository
    {
        IQueryable<Country> GetAll();
        Country GetById(int id);
    }
}
