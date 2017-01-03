using AcspNet.Owin;
using Owin;

namespace WebApplication3
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAcspNet();
        }
    }
}