using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;

namespace InvestNetwork.Core
{
    public class CustomWebRequestErrorEvent : WebRequestErrorEvent
    {
        public CustomWebRequestErrorEvent(string message, object eventSource, int eventCode, Exception exception)
            : base(message, eventSource, eventCode, exception) { }

        public CustomWebRequestErrorEvent(string message, object eventSource, int eventCode, int eventDetailCode, Exception exception)
            : base(message, eventSource, eventCode, eventDetailCode, exception) { }
    }
}
