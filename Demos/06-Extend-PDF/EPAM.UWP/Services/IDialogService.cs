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

using Expenses.UWP.Models;
using System.Threading.Tasks;

namespace Expenses.UWP.Services
{
    public interface IDialogService
    {
        Task<bool> ShowDialogWithOkCancelAsync(string title, string message, string okButton, string cancelButton);
        Task<PdfCreationOption> ShowDialogWithPdfOptions(string title, string message);
        Task ShowDialogAsync(string title, string message);
    }
}
