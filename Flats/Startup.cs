using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Flats.Startup))]
namespace Flats
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
