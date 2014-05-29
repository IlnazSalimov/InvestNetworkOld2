using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class ProjectNewMetaData
    {
        [Display(Name = "Заголовок")]
        public string NewsTittle { get; set; }

        [Display(Name = "Содержание")]
        public string Description { set; get; }
    }
}