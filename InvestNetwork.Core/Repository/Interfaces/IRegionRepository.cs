using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    public interface IRegionRepository
    {
        IQueryable<Region> GetAll();
        Region GetById(int id);
    }
}
