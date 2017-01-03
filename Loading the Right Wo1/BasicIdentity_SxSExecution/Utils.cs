using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Runtime.DurableInstancing;
using System.Threading;


namespace BasicIdentity_SxSExecution
{
    public static class Utils
    {
        public static InstanceStore SetupInstanceStore()
        {
            //***********Important Notice*************
            //You need to modify the following line of code before running the sample.
            //Replace the value of myConnectionString with the value of the "Connection String" property 
            //of the WorkflowInstanceStore database on your own machine.  
            string myConnectionString = @"Data Source=localhost;Initial Catalog=PersistenceDB;Integrated Security=True";
            InstanceStore instanceStore =new SqlWorkflowInstanceStore(myConnectionString);

            InstanceHandle handle = instanceStore.CreateInstanceHandle();
            InstanceView view = instanceStore.Execute(handle, new CreateWorkflowOwnerCommand(), TimeSpan.FromSeconds(30));
            handle.Free();

            instanceStore.DefaultInstanceOwner = view.InstanceOwner;

            return instanceStore;
        }

        public static void ShowMessageFromHost(string message, ConsoleColor color = ConsoleColor.Yellow)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine("[Host] {0}", message);
            Console.ForegroundColor = currentColor;
        }

        public static void LoadInstanceAndResumeBookmark(WorkflowApplication wfApplication, Guid instanceId, string bookmarkName, string bookmarkData)
        {
            // configure the workflow application
            AutoResetEvent syncEvent = new AutoResetEvent(false);
            wfApplication.Unloaded = (e) => syncEvent.Set();
            wfApplication.Completed = (e) => Utils.ShowMessageFromHost(string.Format("WorkflowApplication Completed with '{0}' state.", e.CompletionState), ConsoleColor.Green);

            // setup the instance store
            wfApplication.InstanceStore = Utils.SetupInstanceStore();

            // load the application (will fail due to version mismatch)                
            wfApplication.Load(instanceId);

            // this resumes the bookmark setup by readline           
            wfApplication.ResumeBookmark(bookmarkName, bookmarkData);
            syncEvent.WaitOne();
        }

        public static Guid StartAndUnloadInstance(WorkflowApplication wfApplication)
        {
            ShowMessageFromHost("Starting instance", ConsoleColor.Green);

            // configure the workflow application and start executing            
            AutoResetEvent syncEvent = new AutoResetEvent(false);
            wfApplication.PersistableIdle = (e) => PersistableIdleAction.Unload;
            wfApplication.Unloaded = (e) =>
            {
                ShowMessageFromHost("Persisted and unloaded from memory");
                syncEvent.Set();
            };

            // setup the instance store
            wfApplication.InstanceStore = Utils.SetupInstanceStore();

            wfApplication.Run();

            syncEvent.WaitOne();

            return wfApplication.Id;
        }
    }
}
