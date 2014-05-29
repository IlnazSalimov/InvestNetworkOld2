using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace InvestNetwork.Core
{
    public enum FundingDurationRanges
    {
        Min = 1,
        Max = 60
    }

    public class ProjectMetaData
    {
        /*[Required]
        [Display(Name = "Месторасположение")]
        public int LocationCityID { get; set; }

        [Required]
        [Display(Name = "Название проекта")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Сфера деятельности")]
        public int ScopeID { get; set; }

        [Required]
        [Display(Name = "Описание")]
        [UIHint("TinyMCE_yourtemplatename"), AllowHtml]
        public string Description { get; set; }

        [Required]
        [MinLength(40, ErrorMessage = "Краткое описание должно быть больше 40 символов")]
        [MaxLength(135, ErrorMessage = "Краткое описание должно быть меньше 135 символов")]
        [Display(Name = "Краткое описание")]
        public string ShortDescription { get; set; }

        [Required]
        [Display(Name = "Необходимое финансирование")]
        public string NecessaryFunding { get; set; }

        [Required]
        [Display(Name = "Изображение проекта")]
        public string LinkToImg { get; set; }

        [Required]
        [Range((int)FundingDurationRanges.Min, (int)FundingDurationRanges.Max, ErrorMessage = "Продолжительность финансирования должна быть в пределах от 1 до 60 дней")]
        [Display(Name = "Продолжительность финансирования")]
        public string FundingDuration { get; set; }*/
    }


}