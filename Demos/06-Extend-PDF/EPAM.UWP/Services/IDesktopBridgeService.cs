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

using Expenses.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;

namespace Expenses.UWP.Services
{
    public interface IDesktopBridgeService
    {
        AppServiceConnection Connection { get; set; }
        Task LaunchFullTrustProcessAsync();
        Task<bool> GeneratePdfAsync(string filename, Employee employee, List<Expense> expenses);
        bool IsLaunchFullTrustProcessSupported();
    }
}
