using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvestNetwork.Core
{
    public class CustomDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;
        public CustomDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType).ToList();
        }
    }
}