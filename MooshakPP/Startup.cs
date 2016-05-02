using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MooshakPP.Startup))]
namespace MooshakPP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
