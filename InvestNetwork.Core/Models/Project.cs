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
    
    public partial class Project
    {
        public Project()
        {
            this.Payments = new HashSet<Payment>();
            this.ProjectComments = new HashSet<ProjectComment>();
            this.ProjectNews = new HashSet<ProjectNew>();
        }
    
        public int ProjectID { get; set; }
        public int AuthorID { get; set; }
        public Nullable<int> LocationCityID { get; set; }
        public string Name { get; set; }
        public int ScopeID { get; set; }
        public string Description { get; set; }
        public string LinkToBusinessPlan { get; set; }
        public string LinkToFinancialPlan { get; set; }
        public string LinkToVideoPresentation { get; set; }
        public string LinkToGuaranteeLetter { get; set; }
        public int ProjectStatusID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string LinkToImg { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> NecessaryFunding { get; set; }
        public string ShortDescription { get; set; }
        public Nullable<int> FundingDuration { get; set; }
        public bool IsInspected { get; set; }
        public string ProjectFilesDirectory { get; set; }
    
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<ProjectComment> ProjectComments { get; set; }
        public virtual ICollection<ProjectNew> ProjectNews { get; set; }
        public virtual ProjectStatus ProjectStatus { get; set; }
        public virtual Scope Scope { get; set; }
    }
}
