using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ORUComSys.Startup))]
namespace ORUComSys {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
