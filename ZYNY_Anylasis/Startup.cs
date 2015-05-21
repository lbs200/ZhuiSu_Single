using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZYNY_Anylasis.Startup))]
namespace ZYNY_Anylasis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
