using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class PartycipationUsersInfo
    {
        public int ProjectId { set; get; }
        public int PaymentId { set; get; }
        public string ProjectName { set; get; }
        public string ProjectStatus { set; get; }
        public decimal Sum { set; get; }
        public string PaymentStatus { set; get; }
        public DateTime PaymentDate { set; get; }
    }
}