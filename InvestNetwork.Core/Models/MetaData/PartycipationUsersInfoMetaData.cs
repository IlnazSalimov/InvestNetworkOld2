using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core.MetaData
{
    /// <summary>
    /// Класс для установки мета-данных соответствующей модели
    /// </summary>
    public class PartycipationUsersInfoMetaData
    {
        [DisplayFormat(ApplyFormatInEditMode = true,
               DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime PaymentDate { set; get; }
    }
}