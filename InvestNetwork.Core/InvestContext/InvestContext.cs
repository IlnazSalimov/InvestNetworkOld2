using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace InvestNetwork.Core
{
    public class InvestContext : IInvestContext
    {
        private const string COOKIENAME = "__AUTH_COOKIE";
        
        public IAuthentication Auth { get; set; }
        private IUserRepository _userRepository;

        public InvestContext(IUserRepository userRepository, IAuthentication authentication)
        {
            this._userRepository = userRepository;
            this.Auth = DependencyResolver.Current.GetService<IAuthentication>();
        }

        public User CurrentUser
        {
            get
            {
                return ((IUserProvider)Auth.CurrentUser.Identity).User;
            }
        }

    }
}
