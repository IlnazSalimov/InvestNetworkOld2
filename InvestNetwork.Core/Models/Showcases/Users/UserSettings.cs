using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    [MetadataType(typeof(UserSettingsMetaData))]
    public class UserSettings
    {
        public string OldPassword { set; get; }
        public string NewPassword { set; get; }
        public string ConfirmPassword { set; get; }
        public bool? PostNotice { set; get; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string Avatar { get; set; }
        public string AboutMyself { get; set; }
    }
}