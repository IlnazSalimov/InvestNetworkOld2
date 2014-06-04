using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу регионов
    /// </summary>
    public interface IRegionRepository
    {
        IQueryable<Region> GetAll();
        Region GetById(int id);
    }
}
