//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Expenses.Data.Models;
using Expenses.Data.Services;
using Expenses.UWP.Services;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using PropertyChanged;

namespace Expenses.UWP.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : ViewModelBase
    {
        private readonly IDatabaseService databaseService;
        private readonly INavigationService navigationService;
        private readonly IDesktopBridgeService desktopBridgeService;

        public ObservableCollection<Employee> Employees { get; set; }

        public Employee SelectedEmployee { get; set; }

        public MainViewModel(IDatabaseService databaseService, INavigationService navigationService, IDesktopBridgeService desktopBridgeService)
        {
            this.databaseService = databaseService;
            this.navigationService = navigationService;
            this.desktopBridgeService = desktopBridgeService;
        }

        public async override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            await databaseService.InitializeDatabaseAsync();
            var result = await databaseService.GetEmployeesAsync();
            Employees = new ObservableCollection<Employee>(result);

            await desktopBridgeService.LaunchFullTrustProcessAsync();
        }

        public void OnSelectEmployee()
        {
            navigationService.Navigate("ExpenseDetail", SelectedEmployee.EmployeeId);
        }
    }
}
