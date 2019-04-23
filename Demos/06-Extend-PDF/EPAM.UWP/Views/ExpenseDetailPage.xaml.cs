using System;

using Expenses.UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Expenses.UWP.Views
{
    public sealed partial class ExpenseDetailPage : Page
    {
        private ExpenseDetailViewModel ViewModel => DataContext as ExpenseDetailViewModel;

        public ExpenseDetailPage()
        {
            InitializeComponent();
        }
    }
}
