using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegistryHive.Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnReadKey(object sender, RoutedEventArgs e)
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Contoso\ContosoExpenses");
            if (key!=null)
            {
                string isFirstRun = key.GetValue("FirstRun").ToString();
                KeyStatus.Text = $"The key has been found! Its value is {isFirstRun}";
            }
            else
            {
                KeyStatus.Text = $"The key has not been found!";
            }
        }
    }
}
