using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoCritic.Startup))]
namespace PhotoCritic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
