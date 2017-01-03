using System;
using System.Activities;
using System.Collections.Generic;


namespace BasicIdentity_SxSExecution
{
    public static class WorkflowDefinitionRepository
    {

        static IDictionary<WorkflowIdentity, Activity> definitions = new Dictionary<WorkflowIdentity, Activity>
            {
                { new WorkflowIdentity("Dev11-Sample",  
                                        new Version(1, 0, 0, 0), null), 
                  new Activity1() 
                },
                { 
                  new WorkflowIdentity("Dev11-Sample", 
                                        new Version(2, 0, 0, 0), null), 
                  new Activity2() 
                }
            };

        public static Activity GetActivity(WorkflowIdentity identity)
        {
            return definitions[identity];
        }
    }
}
