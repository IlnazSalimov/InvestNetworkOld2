using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InvestNetwork.Api
{
    public class LocationController : ApiController
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ICityRepository _cityRepository;
        public LocationController(ICountryRepository countryRepository, IRegionRepository regionRepository, ICityRepository cityRepository)
        {
            this._countryRepository = countryRepository;
            this._regionRepository = regionRepository;
            this._cityRepository = cityRepository;
        }

        public List<CountryDTO> GetAllContries()
        {
            return _countryRepository.GetAll().Select(c => new CountryDTO
            {
                CountryID = c.CountryID,
                CountryName = c.CountryName
            }).ToList();
        }

        public List<RegionDTO> GetCountryRegions(int id)
        {
            return _regionRepository.GetAll().Select(r => new RegionDTO
            {
                RegionID = r.RegionID,
                RegionName = r.RegionName,
                CountryID = r.CountryID
            }).Where(r => r.CountryID == id).ToList();
        }

        public List<CityDTO> GetRegionCities(int id)
        {
            return _cityRepository.GetAll().Select(c => new CityDTO
            {
                CityID = c.CityID,
                CityName = c.CityName,
                CountryID = c.CountryID,
                RegionID = c.RegionID
            }).Where(c => c.RegionID == id).ToList();
        }

        public CityDTO GetCityById(int id)
        {
            City city = _cityRepository.GetById(id);
            return new CityDTO
            {
                CityID = city.CityID,
                CityName = city.CityName,
                RegionID = city.RegionID,
                CountryID = city.CountryID
            };
        }
    }
}
