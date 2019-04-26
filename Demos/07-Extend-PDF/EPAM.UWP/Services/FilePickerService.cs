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
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Expenses.UWP.Services
{
    public class FilePickerService : IFilePickerService
    {
        public async Task<StorageFile> SaveFileAsync()
        {
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedFileName = "expense.pdf";
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("PDF", new List<string> { ".pdf" });
            var file = await picker.PickSaveFileAsync();
            return file;
        }
    }
}
