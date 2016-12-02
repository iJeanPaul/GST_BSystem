using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GST_Badge_System.MVC.Startup))]
namespace GST_Badge_System.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
