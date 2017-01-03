using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WCFTestHelper
{
    /// <summary>
    /// Allows you to start/stop the ASP.NET Web Development Server when unit testing WCF Services
    /// </summary>
    public class WCFWebDevServer40 : TestServerHelper
    {
        public WCFWebDevServer40(int port)
        {
            serverPort = port;
        }

        private int serverPort;

        private const string WebDevServer40ProcessName = "WebDev.WebServer40";
        private const string WebDevWindowCaptionFormat = "ASP.NET Development Server - Port {0}";
        private readonly static string WebDevServer40Path = Environment.ExpandEnvironmentVariables(@"%CommonProgramFiles(x86)%\Microsoft Shared\DevServer\10.0\Webdev.WebServer40.exe");
        protected override string ServerWindowCaption
        {
            get { return string.Format(WebDevWindowCaptionFormat, serverPort); }
        }

        protected override string ServerProcessName
        {
            get { return WebDevServer40ProcessName; }
        }

        public static void EnsureIsRunning(int port, string physicalPath, string virtualPath = null)
        {
            WCFWebDevServer40 server = new WCFWebDevServer40(port);
            server.EnsureIsRunning(
                () =>
                {
                    // Start ASP.NET Development Server
                    StringBuilder webServerArgs = new StringBuilder();

                    webServerArgs.AppendFormat("/port:{0} /path:\"{1}\"", port, physicalPath);

                    if (virtualPath != null)
                        webServerArgs.AppendFormat(" /vpath:{0}", virtualPath);

                    Debug.WriteLine("Starting {0} {1}", WebDevServer40Path, webServerArgs);
                    Assert.IsNotNull(Process.Start(WebDevServer40Path, webServerArgs.ToString()));
                });
        }

        public static void Close(int port)
        {
            WCFWebDevServer40 server = new WCFWebDevServer40(port);
            server.CloseIfRunning();
        }
    }
}
