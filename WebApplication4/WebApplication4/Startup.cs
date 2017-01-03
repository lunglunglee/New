using AcspNet.Owin;
using Owin;

namespace WebApplication4
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAcspNet();
        }
    }
}