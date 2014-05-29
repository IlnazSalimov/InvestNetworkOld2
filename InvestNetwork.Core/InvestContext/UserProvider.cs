using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace InvestNetwork.Core
{
    public class UserProvider : IPrincipal
    {
        private UserIndentity userIdentity { get; set; }

        public IIdentity Identity
        {
            get
            {
                return userIdentity;
            }
        }

        public bool IsInRole(string role)
        {
            if (userIdentity.User == null)
            {
                return false;
            }
            else
            {
                return userIdentity.User.InRoles(role);
            }
            
        }


        public UserProvider(string name, IUserRepository repository)
        {
            userIdentity = new UserIndentity();
            userIdentity.Init(name, repository);
        }


        public override string ToString()
        {
            return userIdentity.Name;
        }
    }
}