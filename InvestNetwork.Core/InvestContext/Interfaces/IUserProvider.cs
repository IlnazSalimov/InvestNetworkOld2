using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    public interface IUserProvider
    {
        User User { get; set; }
    }
}
