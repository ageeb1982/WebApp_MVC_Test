using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApp_Test.Startup))]
namespace WebApp_Test
{

    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {/// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
