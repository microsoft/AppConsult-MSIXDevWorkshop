using ExpenseItDemo.Data.Model;
using ExpenseItDemo.Data.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace ExpenseItDemo.Export
{
    class Program
    {
        static AppServiceConnection connection = null;
        static AutoResetEvent appServiceExit;

        static void Main(string[] args)
        {
            //we use an AutoResetEvent to keep to process alive until the communication channel established by the App Service is open
            appServiceExit = new AutoResetEvent(false);
            Thread appServiceThread = new Thread(new ThreadStart(ThreadProc));
            appServiceThread.Start();
            appServiceExit.WaitOne();
        }

        static async void ThreadProc()
        {
            //we create a connection with the App Service defined by the UWP app
            connection = new AppServiceConnection();
            connection.AppServiceName = "DatabaseService";
            connection.PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;
            connection.RequestReceived += Connection_RequestReceived;
            connection.ServiceClosed += Connection_ServiceClosed;

            //we open the connection
            AppServiceConnectionStatus status = await connection.OpenAsync();

            if (status != AppServiceConnectionStatus.Success)
            {
                //if the connection fails, we terminate the Win32 process
                appServiceExit.Set();
            }
            else
            {
                //if the connection is succesfull, we communicate to the UWP app that the channel has been established
                ValueSet initialStatus = new ValueSet();
                initialStatus.Add("Status", "Ready");
                await connection.SendMessageAsync(initialStatus);
            }
        }

        private static void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            //when the connection with the App Service is closed, we terminate the Win32 process
            appServiceExit.Set();
        }

        private static async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            if (args.Request.Message["GetEmployees"].ToString() == "true")
            {
                DatabaseService service = new DatabaseService();
                List<Employee> employees = service.GetEmployees();

                string serializedValue = JsonConvert.SerializeObject(employees);

                ValueSet set = new ValueSet();
                set.Add("Employees", serializedValue);

                await args.Request.SendResponseAsync(set);
            }
        }
    }
}
