using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvestNetwork.Core
{
    public class ProjectStartingSecondStep
    {
        [Required]
        public int ProjectID { get; set; }

        /*[Required]
        public string LinkToBusinessPlan { get; set; }
        [Required]
        public string LinkToFinancialPlan { get; set; }
        [Required]
        public string LinkToVideoPresentation { get; set; }
        [Required]
        public string LinkToGuaranteeLetter { get; set; }*/

        [Required]
        [Display(Name = "Описание")]
        [UIHint("TinyMCE_yourtemplatename"), AllowHtml]
        public string Description { get; set; }

        [Required]
        public string ProjectFilesDirectory { get; set; }

        [Required]
        [Display(Name = "Изображение проекта")]
        public string LinkToImg { get; set; }
    }
}