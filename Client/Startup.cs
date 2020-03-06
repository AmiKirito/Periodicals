using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Client.Startup))]

namespace Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
