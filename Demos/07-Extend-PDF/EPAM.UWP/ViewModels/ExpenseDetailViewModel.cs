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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Expenses.Data.Models;
using Expenses.Data.Services;
using Expenses.UWP.Services;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using PropertyChanged;
using Windows.Storage;
using Windows.System;
using System;
using Expenses.UWP.Models;

namespace Expenses.UWP.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ExpenseDetailViewModel : ViewModelBase
    {
        private readonly IDatabaseService databaseService;
        private readonly IDesktopBridgeService desktopBridgeService;
        private readonly IFilePickerService filePickerService;
        private readonly IDialogService dialogService;
        private readonly IShareService shareService;

        public ObservableCollection<Expense> Expenses { get; set; }

        public Employee CurrentEmployee { get; set; }

        public bool IsExportAsPdfAvailable { get; set; }

        public bool IsBusy { get; set; }

        public ExpenseDetailViewModel(IDatabaseService databaseService, IDesktopBridgeService desktopBridgeService, IFilePickerService filePickerService, IDialogService dialogService, IShareService shareService)
        {
            this.databaseService = databaseService;
            this.desktopBridgeService = desktopBridgeService;
            this.filePickerService = filePickerService;
            this.dialogService = dialogService;
            this.shareService = shareService;
        }

        public async override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            int selectedEmployee = (int)e.Parameter;
            var result = await databaseService.GetExpensesAsync(selectedEmployee);
            Expenses = new ObservableCollection<Expense>(result);
            CurrentEmployee = await databaseService.GetEmployeeAsync(selectedEmployee);

            IsExportAsPdfAvailable = desktopBridgeService.IsLaunchFullTrustProcessSupported();
        }

        public async void OnExportAsPdf()
        {
            bool hasOperationFailed = false;

            StorageFile file = await filePickerService.SaveFileAsync();
            IsBusy = true;
            if (file != null)
            {
                bool isSuccess = await desktopBridgeService.GeneratePdfAsync(file.Path, CurrentEmployee, new List<Expense>(Expenses));
                if (isSuccess)
                {
                    PdfCreationOption result = await dialogService.ShowDialogWithPdfOptions("PDF creation", "The PDF has been created with success! What do you want to do?");
                    if (result == PdfCreationOption.Open)
                    {
                        await Launcher.LaunchFileAsync(file);
                    }
                    else if (result == PdfCreationOption.Share)
                    {
                        shareService.Share("EPAM", "Expense report", file);
                    }
                }
                else
                {
                    hasOperationFailed = true;
                }
            }
            else
            {
                hasOperationFailed = true;
            }

            if (hasOperationFailed)
            {
                await dialogService.ShowDialogAsync("PDF creation", "We couldn't create the PDF, try again later");
            }
            
            IsBusy = false;
        }
    }
}
