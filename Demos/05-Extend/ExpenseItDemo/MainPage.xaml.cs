using ExpenseItDemo.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ExpenseItDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ApiInformation.IsTypePresent("Windows.ApplicationModel.FullTrustProcessLauncher"))
            {
                //we launch the Win32 process that generates the boarding pass on the desktop
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
        }

        private async void OnGetEmployees(object sender, RoutedEventArgs e)
        {
            //if the connection with the App Service has been established, we send the info about the flight to the Win32 process
            if (App.Connection != null)
            {
                ValueSet set = new ValueSet();
                set.Add("GetEmployees", "true");

                AppServiceResponse response = await App.Connection.SendMessageAsync(set);
                //if the Win32 process has received the data and it has successfully generated the boarding pass, we show a notification to the user
                if (response.Status == AppServiceResponseStatus.Success)
                {
                    string message = response.Message["Employees"].ToString();
                    List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(message);
                    EmployeesView.ItemsSource = employees;
                }
            }
        }
    }
}
