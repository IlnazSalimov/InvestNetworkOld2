using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public partial class PaymentStatus : IEntity
    {
        public int ID
        {
            get { return this.PaymentStatusID; }
        }
    }
}