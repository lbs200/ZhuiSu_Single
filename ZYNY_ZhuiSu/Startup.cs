using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZYNY_ZhuiSu.Startup))]
namespace ZYNY_ZhuiSu
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
