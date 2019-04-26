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
using System.Threading.Tasks;
using Expenses.UWP.Models;
using Windows.UI.Popups;

namespace Expenses.UWP.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowDialogAsync(string title, string message)
        {
            MessageDialog dialog = new MessageDialog(message, title);
            await dialog.ShowAsync();
        }

        public async Task<bool> ShowDialogWithOkCancelAsync(string title, string message, string okButton, string cancelButton)
        {
            MessageDialog dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand(okButton));
            dialog.Commands.Add(new UICommand(cancelButton));
            var result = await dialog.ShowAsync();
            if (result.Label == okButton)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PdfCreationOption> ShowDialogWithPdfOptions(string title, string message)
        {
            MessageDialog dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand("Open"));
            dialog.Commands.Add(new UICommand("Share"));
            dialog.Commands.Add(new UICommand("Nothing"));
            var result = await dialog.ShowAsync();
            if (result.Label == "Open")
            {
                return PdfCreationOption.Open;
            }
            else if (result.Label == "Share")
            {
                return PdfCreationOption.Share;
            }
            else
            {
                return PdfCreationOption.Nothing;
            }
        }
    }
}
