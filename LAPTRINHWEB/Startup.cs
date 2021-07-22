using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LAPTRINHWEB.Startup))]
namespace LAPTRINHWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
