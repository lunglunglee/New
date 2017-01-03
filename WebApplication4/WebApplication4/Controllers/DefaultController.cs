using AcspNet;
using AcspNet.Attributes;
using AcspNet.Responses;

namespace WebApplication4.Controllers
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