using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InvestNetwork.Core;

namespace InvestNetwork.Core
{
    public partial class ProjectNewsComment : IEntity
    {
        public int ID
        {
            get { return this.ProjectNewsCommentID; }
        }

        public User User { set; get; }
    }
}