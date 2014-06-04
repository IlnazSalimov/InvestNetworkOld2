using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Предоставляет методы, которые предоставляют доступ к хранилищу ролей
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        private IRepository<Role> roleRepository;

        public RoleRepository(IRepository<Role> userRepository)
        {
            this.roleRepository = userRepository;
        }
        public IQueryable<Role> GetAll()
        {
            return roleRepository.GetAll();
        }

        public Role GetById(int id)
        {
            if (id == 0)
                return null;
            return roleRepository.GetById(id);
        }

        public void Insert(Role model)
        {
            if (model == null)
                throw new ArgumentNullException("role");
            roleRepository.Insert(model);
        }

        public void Update(Role model)
        {
            if (model == null)
                throw new ArgumentNullException("role");
            roleRepository.Update(model);

        }

        public void Delete(Role model)
        {
            if (model == null)
                throw new ArgumentNullException("role");
            roleRepository.Delete(model);
        }

        public void SaveChanges()
        {
            roleRepository.SaveChanges();
        }
    }
}