
#region Imports
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.ServiceModel.Web;
#endregion

namespace ReSTAuthLibrary
{
    /// <summary>
    /// Custom ServiceHostFactory that injects the interceptor when the WebServiceHost2 instance is first created
    /// </summary>  
    public class SecureWebServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            WebServiceHost2 host = new WebServiceHost2(serviceType, true, baseAddresses);
            host.Interceptors.Add(new AuthInterceptor());
            return host;
        }
    }
}
