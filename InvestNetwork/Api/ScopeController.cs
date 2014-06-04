using InvestNetwork.Controllers;
using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace InvestNetwork.Api
{
    /// <summary>
    /// Предоставляет методы, организующие интерфейс управления списком сфер деятельности"</summary>
    public class ScopeController : ApiController
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о списке сфер деятельности.</summary>
        private readonly IScopeRepository _scopeRepository;

        /// <summary>  
        /// Инициализирует новый экземпляр LocationController с внедрением зависемостей к хранилищам.</summary>  
        /// <param name="scopeRepository">Экземпляр класса ScopeRepository, предоставляющий доступ к хранилищу данных о списке сфер деятельности.</param>
        public ScopeController(IScopeRepository scopeRepository)
        {
            this._scopeRepository = scopeRepository;
        }

        /// <summary>  
        /// Возвращает список всех сфер деятельности.</summary>
        /// <returns>Список экземпляров ScopeDTO</returns>
        public List<ScopeDTO> GetAllScopes()
        {
            return _scopeRepository.GetAll().Select(s => new ScopeDTO
            {
                ScopeID = s.ScopeID,
                Title = s.Title
            }).ToList();
        }
    }
}
