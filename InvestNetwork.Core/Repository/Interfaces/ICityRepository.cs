using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу городов
    /// </summary>
    public interface ICityRepository
    {
        IQueryable<City> GetAll();
        City GetById(int id);
    }
}
