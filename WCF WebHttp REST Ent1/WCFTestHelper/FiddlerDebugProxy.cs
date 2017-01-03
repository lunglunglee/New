using System;

namespace WCFTestHelper
{
    /// <summary>
    /// Allows you to start/stop Fiddler HTTP Debugging Proxy http://www.fiddler2.com when testing WCF Services
    /// </summary>
    public class FiddlerDebugProxy : TestServerHelper
    {
        private const string FiddlerServerProcessName = "Fiddler.WebServer";
        private const string FiddlerWindowCaption = "Fiddler - HTTP Debugging Proxy";
        private const int FiddlerWarmUpDelay = 2000;

        private readonly static string FiddlerServerPath = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\Fiddler2\Fiddler.exe");

        protected override string ServerWindowCaption
        {
            get { return FiddlerWindowCaption; }
        }

        protected override string ServerProcessName
        {
            get { return FiddlerServerProcessName; }
        }

        public static void EnsureIsRunning()
        {
            FiddlerDebugProxy server = new FiddlerDebugProxy();
            server.EnsureIsRunning(FiddlerServerPath, FiddlerWarmUpDelay);
        }

        public static void Close()
        {
            FiddlerDebugProxy server = new FiddlerDebugProxy();
            server.CloseIfRunning();
        }
    }
}
