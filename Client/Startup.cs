using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Periodicals.Startup))]

namespace Periodicals
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
