using AcspNet;
using AcspNet.Attributes;
using AcspNet.Responses;

namespace WebApplication3.Controllers
{
    [Get("/")]
    public class DefaultController : Controller
    {
        public override ControllerResponse Invoke()
        {
            return new Tpl("Hello world!");
        }
    }
}
