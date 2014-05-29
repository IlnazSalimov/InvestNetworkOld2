using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InvestNetwork.Startup))]
namespace InvestNetwork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
