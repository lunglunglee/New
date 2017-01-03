using AcspNet.Owin;
using Owin;

namespace WebApplication2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAcspNet();
        }
    }
}