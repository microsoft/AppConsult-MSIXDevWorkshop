using System;

using Expenses.UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Expenses.UWP.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
