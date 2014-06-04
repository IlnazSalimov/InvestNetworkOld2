using Ninject;
using Ninject.Web;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.WebHost;
using System.Web.Http;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Предоставляет методы конфигурации разрешения зависемостей
    /// </summary>
    public class DependencyConfigure
    {
        /// <summary>
        /// Инициализация и конфигурирование контейнера IoC. Регистрация зависемостей.
        /// </summary>
        public static void Initialize()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IDataContext>().To<InvestNetworkEntities>().InRequestScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<ICountryRepository>().To<CountryRepository>();
            kernel.Bind<IRegionRepository>().To<RegionRepository>();
            kernel.Bind<ICityRepository>().To<CityRepository>();
            kernel.Bind<IProjectRepository>().To<ProjectRepository>();
            kernel.Bind<IScopeRepository>().To<ScopeRepository>();
            kernel.Bind<IProjectStatusRepository>().To<ProjectStatusRepository>();
            kernel.Bind<IInvestContext>().To<InvestContext>();
            kernel.Bind<IRoleRepository>().To<RoleRepository>();
            kernel.Bind<IUsersInfoRepository>().To<UsersInfoRepository>();
            kernel.Bind<IAuthentication>().To<CustomAuthentication>().InRequestScope();
            kernel.Bind<IMessageRepository>().To<MessageRepository>();
            kernel.Bind<IProjectCommentRepository>().To<ProjectCommentRepository>();
            kernel.Bind<IProjectNewsRepository>().To<ProjectNewsRepository>();
            kernel.Bind<IMapper>().To<CommonMapper>();
            kernel.Bind<IProjectNewsCommentsRepository>().To<ProjectNewsCommentsRepository>();

            DependencyResolver.SetResolver(new CustomDependencyResolver(kernel));
            GlobalConfiguration.Configuration.DependencyResolver =
                new NinjectWebApiResolver(kernel);
        }
    }
}