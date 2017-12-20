using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BinkodApp.Web.Startup))]
namespace BinkodApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

            BinkodApp.Web.App_Start.HangfireConfig.ConfigureHangfire(app);
            //BinkodApp.Web.App_Start.HangfireConfig.InitializeJobs();
        }
    }
}
