using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class CityRepository : ICityRepository
    {
        private IRepository<City> cityRepository;

        public CityRepository(IRepository<City> cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        public IQueryable<City> GetAll()
        {
            return cityRepository.GetAll();
        }

        public City GetById(int id)
        {
            if (id == 0)
                return null;
            return cityRepository.GetById(id);
        }
    }
}