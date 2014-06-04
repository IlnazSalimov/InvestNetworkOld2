using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InvestNetwork.Api
{
    /// <summary>
    /// Предоставляет методы, организующие интерфейс управления локацией</summary>
    public class LocationController : ApiController
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о странах.</summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о регионах.</summary>
        private readonly IRegionRepository _regionRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о городах.</summary>
        private readonly ICityRepository _cityRepository;

        /// <summary>  
        /// Инициализирует новый экземпляр LocationController с внедрением зависемостей к хранилищам.</summary>  
        /// <param name="countryRepository">Экземпляр класса CountryRepository, предоставляющий доступ к хранилищу данных о странах.</param>
        /// <param name="regionRepository">Экземпляр класса RegionRepository, предоставляющий доступ к хранилищу данных о регионах.</param>
        /// <param name="cityRepository">Экземпляр класса CityRepository, предоставляющий доступ к хранилищу данных о городах.</param>
        public LocationController(ICountryRepository countryRepository, IRegionRepository regionRepository, ICityRepository cityRepository)
        {
            this._countryRepository = countryRepository;
            this._regionRepository = regionRepository;
            this._cityRepository = cityRepository;
        }

        /// <summary>  
        /// Возвращает список всех стран.</summary>
        /// <returns>Список экземпляров CountryDTO</returns>
        public List<CountryDTO> GetAllContries()
        {
            return _countryRepository.GetAll().Select(c => new CountryDTO
            {
                CountryID = c.CountryID,
                CountryName = c.CountryName
            }).ToList();
        }

        /// <summary>  
        /// Возвращает список всех регионов, которые принадлежат стране с заданным идентификатором.</summary>
        /// <returns>Список экземпляров RegionDTO</returns>
        public List<RegionDTO> GetCountryRegions(int id)
        {
            return _regionRepository.GetAll().Select(r => new RegionDTO
            {
                RegionID = r.RegionID,
                RegionName = r.RegionName,
                CountryID = r.CountryID
            }).Where(r => r.CountryID == id).ToList();
        }

        /// <summary>  
        /// Возвращает список всех городов, которые принадлежат региону с заданным идентификатором.</summary>
        /// <returns>Список экземпляров CityDTO</returns>
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

        /// <summary>  
        /// Возвращает город с заданным идентификатором.</summary>
        /// <returns>Экземпляр CityDTO</returns>
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
