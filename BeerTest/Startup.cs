using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BeerTest.Startup))]
namespace BeerTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
