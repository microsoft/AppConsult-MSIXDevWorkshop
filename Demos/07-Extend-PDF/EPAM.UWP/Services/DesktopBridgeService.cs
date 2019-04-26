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
using System.Threading.Tasks;
using Expenses.Data.Models;
using Expenses.UWP.Helpers;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;

namespace Expenses.UWP.Services
{
    public class DesktopBridgeService : IDesktopBridgeService
    {
        public AppServiceConnection Connection { get; set; }

        public async Task<bool> GeneratePdfAsync(string filename, Employee employee, List<Expense> expenses)
        {
            if (Connection != null)
            {
                ValueSet data = new ValueSet();
                data.Add("CreatePdf", "true");
                data.Add("Filename", filename);

                string employeeJson = await Json.StringifyAsync(employee);
                data.Add("Employee", employeeJson);

                string expensesJson = await Json.StringifyAsync(expenses);
                data.Add("Expenses", expensesJson);

                var response = await Connection.SendMessageAsync(data);

                if (response.Status == AppServiceResponseStatus.Success)
                {
                    bool isSuccess = (bool)response.Message["Completed"];
                    return isSuccess;
                }
            }

            return false;
        }

        public bool IsLaunchFullTrustProcessSupported()
        {
            return ApiInformation.IsTypePresent("Windows.ApplicationModel.FullTrustProcessLauncher");
        }

        public async Task LaunchFullTrustProcessAsync()
        {
            if (IsLaunchFullTrustProcessSupported())
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
        }
    }
}
