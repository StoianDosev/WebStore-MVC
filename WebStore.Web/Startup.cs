using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebStore.Web.Startup))]
namespace WebStore.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
