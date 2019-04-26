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

namespace Expenses.Data.Services
{
    public interface IDatabaseService
    {
        Task InitializeDatabaseAsync();
        Task<List<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeAsync(int employeeId);
        Task <List<Expense>> GetExpensesAsync(int employeedId);
    }
}
