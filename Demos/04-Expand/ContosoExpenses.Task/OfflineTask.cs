using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Networking.Connectivity;
using Windows.UI.Notifications;

namespace ContosoExpenses.Task
{
    public sealed class OfflineTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            var profile = NetworkInformation.GetInternetConnectionProfile();

            string title;
            string description;

            if (profile != null)
            {
                title = "You're online!";
                description = "Make sure to sync all the expenses for your trip!";
            }
            else
            {
                title = "You're offline!";
                description = "Your expenses won't be synced until you're back online.";
            }


            string xml = $@"<toast>
            <visual>
                <binding template='ToastGeneric'>
                    <text>{title}</text>
                    <text>{description}</text>
                </binding>
            </visual>
        </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            ToastNotification toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);

            deferral.Complete();
        }
    }
}