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
using MigraDoc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace Expenses.PDFCreator
{
    class Program
    {
        static AppServiceConnection connection = null;
        static AutoResetEvent appServiceExit;

        static void Main(string[] args)
        {
            appServiceExit = new AutoResetEvent(false);
            Thread appServiceThread = new Thread(new ThreadStart(ThreadProc));
            appServiceThread.Start();
            appServiceExit.WaitOne();
        }

        static async void ThreadProc()
        {
            //we create a connection with the App Service defined by the UWP app
            connection = new AppServiceConnection();
            connection.AppServiceName = "PdfService";
            connection.PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;
            connection.RequestReceived += Connection_RequestReceived;
            connection.ServiceClosed += Connection_ServiceClosed;

            //we open the connection
            AppServiceConnectionStatus status = await connection.OpenAsync();

            if (status != AppServiceConnectionStatus.Success)
            {
                //if the connection fails, we terminate the Win32 process
                appServiceExit.Set();
            }
        }

        private static void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            //when the connection with the App Service is closed, we terminate the Win32 process
            appServiceExit.Set();
        }

        private static async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            if (args.Request.Message["CreatePdf"].ToString() == "true")
            {
                bool isSuccess;

                string filename = args.Request.Message["Filename"].ToString();
                Employee employee = JsonConvert.DeserializeObject<Employee>(args.Request.Message["Employee"].ToString());
                List<Expense> expenses = JsonConvert.DeserializeObject<List<Expense>>(args.Request.Message["Expenses"].ToString());

                try
                {
                    ExpenseForm form = new ExpenseForm(filename, employee, expenses);
                    var document = form.CreateDocument();

                    PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                    pdfRenderer.Document = document;
                    pdfRenderer.RenderDocument();
                    pdfRenderer.PdfDocument.Save(filename);

                    isSuccess = true;
                }
                catch (Exception exc)
                {
                    isSuccess = false;
                }

                ValueSet set = new ValueSet();
                set.Add("Completed", isSuccess);

                await args.Request.SendResponseAsync(set);
            }
        }
    }
}
