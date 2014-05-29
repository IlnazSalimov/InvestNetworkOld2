using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    [MetadataType(typeof(SignupUserMetaData))]
    public class SignupUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Captcha { get; set; }
        public string FullName { get; set; }
        public bool RememberMe { get; set; }
    }
}