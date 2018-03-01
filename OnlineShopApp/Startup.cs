using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineShopApp.Startup))]
namespace OnlineShopApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
