using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Предоставляет методы, которые предоставляют доступ к хранилищу сфер деятельности
    /// </summary>
    public class ScopeRepository : IScopeRepository
    {
        private IRepository<Scope> scopeRepository;

        public ScopeRepository(IRepository<Scope> scopeRepository)
        {
            this.scopeRepository = scopeRepository;
        }
        public IQueryable<Scope> GetAll()
        {
            return scopeRepository.GetAll();
        }

        public Scope GetById(int id)
        {
            if (id == 0)
                return null;
            return scopeRepository.GetById(id);
        }

        public void Insert(Scope model)
        {
            if (model == null)
                throw new ArgumentNullException("scope");
            scopeRepository.Insert(model);
        }
    }
}