using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Salutem_Version3.Startup))]
namespace Salutem_Version3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
