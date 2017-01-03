using System;
using System.Activities;
using System.Threading;    


namespace BasicIdentity_SxSExecution
{  
    public class SxSExecution
    {
        public static void RunScenario()
        {
            //***********************************************************
            // STEP 1: create a WorkflowApplication with a definition,
            //         and an identity, start an instance, and persist it and 
            //         unload it when it gets idle
            //***********************************************************

            // Create a workflow application using identity
            // notice that the delta between WF4 is the following:
            //    - create an instance of WorkflowIdentity
            //    - pass it to WorkflowApplication in its constructor
            WorkflowIdentity identityV1 =
                              new WorkflowIdentity("Dev11-Sample",
                                                  new Version(1, 0, 0, 0), null);
            WorkflowApplication application = new WorkflowApplication(new Activity1(), identityV1);

            // create the instance store and assign it to the workflow application            
            application.InstanceStore = Utils.SetupInstanceStore();

            // start an instance, unload it, and persist it into the instance store
            Guid instanceId = Utils.StartAndUnloadInstance(application);

            // read input from the console
            string input = Console.ReadLine();


            //***********************************************************
            // STEP 2: given an instance id, look up the definition in the
            //         instance store, configure a workflow application
            //         load the instance, and continue with its execution
            //***********************************************************
            // load the instance with the correct definition
            WorkflowApplicationInstance instance = null;
            WorkflowApplication newApplication = null;            
            AutoResetEvent syncEvent = new AutoResetEvent(false);

            try
            {
                // get a proxy to the running instance
                instance = WorkflowApplication.GetInstance(instanceId, Utils.SetupInstanceStore());

                // get the definition associated to the identity of the instance
                // for this we are using a dictionary with key=identity; value=activity
                Activity definition = WorkflowDefinitionRepository.GetActivity(instance.DefinitionIdentity);

                // create and configure the workflowApplication
                newApplication = new WorkflowApplication(definition, instance.DefinitionIdentity);

                // configure the workflow application                
                newApplication.Completed = (e) => Utils.ShowMessageFromHost(string.Format("WorkflowApplication has Completed in the {0} state.", e.CompletionState), ConsoleColor.Green);
                newApplication.Unloaded = (e) => syncEvent.Set();

                // show messages in the console
                Utils.ShowMessageFromHost("Loading the workflow using its identity in the InstanceStore...", ConsoleColor.Cyan);
                Utils.ShowMessageFromHost(string.Format("Loading with identity '{0}'", instance.DefinitionIdentity.ToString()));

                // load the application (passing the instance that we got previously)
                newApplication.Load(instance);

                //this resumes the bookmark setup by readline           
                newApplication.ResumeBookmark("ReadLine", input);
                syncEvent.WaitOne();
            }
            catch
            {
                // if there is an error, discard the instance so it does not remain locked
                if (newApplication != null && instance != null)
                {
                    instance.Abandon();
                }
            }
        }                               
    }  

}
