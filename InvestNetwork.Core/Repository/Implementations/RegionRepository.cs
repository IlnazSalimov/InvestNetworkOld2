using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class RegionRepository:IRegionRepository
    {
        private IRepository<Region> regionRepository;

        public RegionRepository(IRepository<Region> regionRepository)
        {
            this.regionRepository = regionRepository;
        }

        public IQueryable<Region> GetAll()
        {
            return regionRepository.GetAll();
        }

        public Region GetById(int id)
        {
            if (id == 0)
                return null;
            return regionRepository.GetById(id);
        }
    }
}