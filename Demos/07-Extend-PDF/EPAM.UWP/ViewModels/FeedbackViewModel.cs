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

using Expenses.UWP.Services;
using Prism.Commands;
using Prism.Windows.Mvvm;
using PropertyChanged;
using System;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Expenses.UWP.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class FeedbackViewModel : ViewModelBase
    {
        private IScreenCaptureService _screenCaptureService;
        private IDialogService _dialogService;

        public FeedbackViewModel(IScreenCaptureService screenCaptureService, IDialogService dialogService)
        {
            _screenCaptureService = screenCaptureService;
            _dialogService = dialogService;
        }

        public string Feedback { get; set; }

        public BitmapImage ScreenshotPreview { get; set; }

        private ICommand _captureScreenCommand;

        public ICommand CaptureScreenCommand
        {
            get
            {
                if (_captureScreenCommand == null)
                {
                    _captureScreenCommand = new DelegateCommand(async () =>
                    {
                        await _screenCaptureService.CaptureScreenshotAsync("screenshot.png");
                        var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appdata:///local/screenshot.png"));
                        using (var stream = await file.OpenReadAsync())
                        {
                            ScreenshotPreview = new BitmapImage();
                            await ScreenshotPreview.SetSourceAsync(stream);
                        }
                    });
                }

                return _captureScreenCommand;
            }
        }

        private ICommand _sendFeedbackCommand;

        public ICommand SendFeedbackCommand
        {
            get
            {
                if (_sendFeedbackCommand == null)
                {
                    _sendFeedbackCommand = new DelegateCommand(async () =>
                    {
                        await _dialogService.ShowDialogAsync("Feedback sent", "Your feedback has been sent succesfully! Thanks for reporting it.");
                    });
                }

                return _sendFeedbackCommand;
            }
        }
    }
}
