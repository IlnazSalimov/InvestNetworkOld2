//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InvestNetwork.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectNew
    {
        public ProjectNew()
        {
            this.ProjectNewsComments = new HashSet<ProjectNewsComment>();
        }
    
        public int ProjectNewsID { get; set; }
        public int ProjectID { get; set; }
        public string Description { get; set; }
        public System.DateTime NewsDate { get; set; }
        public string NewsTittle { get; set; }
    
        public virtual Project Project { get; set; }
        public virtual ICollection<ProjectNewsComment> ProjectNewsComments { get; set; }
    }
}