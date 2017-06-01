using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestProfiGroup.Startup))]
namespace TestProfiGroup
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
