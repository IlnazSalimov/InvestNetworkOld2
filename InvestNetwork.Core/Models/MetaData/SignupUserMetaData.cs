using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class SignupUserMetaData
    {
        [Required]
        [Display(Name = "E-mail")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Некорректный E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Введите пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Введите код с картинки")]
        public string Captcha { get; set; }

        [Required]
        [Display(Name = "Имя профиля")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}