using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Background;

namespace ContosoExpenses
{
    public class BackgroundTaskService
    {
        public void RegisterBackgroundTask()
        {
            string triggerName = "NetworkTrigger";

            // Check if the task is already registered
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                cur.Value.Unregister(true);
            }

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.Name = triggerName;
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
            builder.TaskEntryPoint = "ContosoExpenses.Task.OfflineTask";
            builder.Register();
        }
    }

        
}
