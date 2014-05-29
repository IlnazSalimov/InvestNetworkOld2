using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class ProjectDTO
    {
        [Required]
        public int ProjectID { get; set; }

        [Required]
        public int AuthorID { get; set; }

        [Required]
        public int LocationCityID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Scope { get; set; }

        [Required]
        public string Description { get; set; }

        public string LinkToBusinessPlan { get; set; }

        public string LinkToFinancialPlan { get; set; }

        public string LinkToVideoPresentation { get; set; }

        public string LinkToGuaranteeLetter { get; set; }

        public string Status { get; set; }

        public DateTime StartDate { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public string LinkToImg { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal NecessaryFunding { get; set; }

        [Required]
        [MinLength(40, ErrorMessage = "Краткое описание должно быть больше 40 символов")]
        [MaxLength(135, ErrorMessage = "Краткое описание должно быть меньше 135 символов")]
        public string ShortDescription { get; set; }

        [Required]
        [Range((int)FundingDurationRanges.Min, (int)FundingDurationRanges.Max, ErrorMessage = "Продолжительность финансирования должна быть в пределах от 1 до 60 дней")]
        public int FundingDuration { get; set; }

        public bool IsInspected { get; set; }

        [Required]
        public string ProjectFilesDirectory { get; set; }
    }
}