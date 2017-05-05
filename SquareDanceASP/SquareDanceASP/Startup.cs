using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SquareDanceASP.Startup))]
namespace SquareDanceASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
