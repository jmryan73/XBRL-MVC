using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XBRLApp.Startup))]
namespace XBRLApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
