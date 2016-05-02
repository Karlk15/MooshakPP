using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mooshak__.Startup))]
namespace Mooshak__
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
