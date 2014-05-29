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
    public class UsersInfoMetaData
    {
        [Required]
        [Display(Name = "Фамилия")]
        public string Family { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string Middle { get; set; }

        [Display(Name = "Дата Рождения")]
        [DisplayFormat(ApplyFormatInEditMode = true, 
               DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Гражданство")]
        public string Citizenship { get; set; }

        [Required]
        [Display(Name = "Серия паспорта")]
        public string PasportSerie { get; set; }

        [Required]
        [Display(Name = "Номер паспорта")]
        public string PasportNumber { get; set; }

        [Display(Name = "Дата выдачи паспорта")]
        [DisplayFormat(ApplyFormatInEditMode = true,
               DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime PasportIssueDate { get; set; }

        [Required]
        [Display(Name = "Кем выдан паспорт")]
        public string PasportIssuedBy { get; set; }

        [Required]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true,
               DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "О себе")]
        public string AboutMyself { get; set; }
    }


}