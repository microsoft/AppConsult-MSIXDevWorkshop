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

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace Expenses.UWP.Services
{
    public class ShareService : IShareService
    {
        private string title;
        private string description;
        private StorageFile file;

        public ShareService()
        {
            DataTransferManager.GetForCurrentView().DataRequested += UwpShareService_DataRequested;
        }

        private void UwpShareService_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            args.Request.Data.Properties.Title = title;
            args.Request.Data.Properties.Description = description;
            args.Request.Data.SetStorageItems(new[] { file });
            deferral.Complete();
        }

        public void Share(string title, string description, StorageFile file)
        {
            this.title = title;
            this.description = description;
            this.file = file;

            DataTransferManager.ShowShareUI();
        }
    }
}
