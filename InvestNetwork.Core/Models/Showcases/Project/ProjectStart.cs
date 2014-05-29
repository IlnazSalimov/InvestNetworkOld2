using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class ProjectStart
    {
        [Required]
        public int ProjectID { get; set; }

        [Required]
        [Display(Name = "Месторасположение")]
        public int LocationCityID { get; set; }

        [Required]
        [Display(Name = "Название проекта")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Сфера деятельности")]
        public int ScopeID { get; set; }

        [Required]
        [MinLength(40, ErrorMessage = "Краткое описание должно быть больше 40 символов")]
        [MaxLength(135, ErrorMessage = "Краткое описание должно быть меньше 135 символов")]
        [Display(Name = "Краткое описание")]
        public string ShortDescription { get; set; }

        [Required]
        [Display(Name = "Необходимое финансирование")]
        public int NecessaryFunding { get; set; }

        [Required]
        [Range((int)FundingDurationRanges.Min, (int)FundingDurationRanges.Max, ErrorMessage = "Продолжительность финансирования должна быть в пределах от 1 до 60 дней")]
        [Display(Name = "Продолжительность финансирования")]
        public int FundingDuration { get; set; }
    }
}