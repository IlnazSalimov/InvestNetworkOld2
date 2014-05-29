using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class UsersInfoDTO
    {
        public int UsersInfoID { get; set; }
        public int UserID { get; set; }
        public string Family { get; set; }
        public string Name { get; set; }
        public string Middle { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Citizenship { get; set; }
        public string PasportSerie { get; set; }
        public string PasportNumber { get; set; }
        public System.DateTime PasportIssueDate { get; set; }
        public string PasportIssuedBy { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime RegisterDate { get; set; }
    }
}