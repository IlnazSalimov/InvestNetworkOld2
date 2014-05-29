using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Класс для установки мета-данных соответствующей модели
    /// </summary>
    public class UserSettingsMetaData
    {
        [Display(Name = "Имя")]
        public string FullName { get; set; }

        [Display(Name = "E-mail")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Некорректный E-mail")]
        public string Email { set; get; }

        [Display(Name = "Старый пароль")]
        public string OldPassword { set; get; }

        [Display(Name = "Новый пароль")]
        public string NewPassword { set; get; }

        [Display(Name = "Подвердите пароль")]
        public string ConfirmPassword { set; get; }

        [Display(Name = "Получать уведомления по почте")]
        public bool PostNotice { set; get; }
    }
}