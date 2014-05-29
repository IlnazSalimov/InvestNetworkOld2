using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InvestNetwork.Core
{
    [MetadataType(typeof(ProjectNewMetaData))]
    public partial class ProjectNew : IEntity
    {
        public int ID
        {
            get { return this.ProjectNewsID; }
        }
    }

}