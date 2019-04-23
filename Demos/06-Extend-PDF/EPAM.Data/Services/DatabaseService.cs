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
using System.Threading.Tasks;
using Bogus;
using Expenses.Data.Models;
using SQLite;

namespace Expenses.Data.Services
{
    public class DatabaseService : IDatabaseService
    {
        private SQLiteAsyncConnection connection;
        readonly int numberOfEmployees = 10;
        readonly int numberOfExpenses = 5;

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            var employee = await connection.Table<Employee>().Where(x => x.EmployeeId == employeeId).FirstOrDefaultAsync();
            return employee;
        }

        public Task<List<Employee>> GetEmployeesAsync()
        {
            var employees = connection.Table<Employee>().ToListAsync();
            return employees;
        }

        public async Task<List<Expense>> GetExpensesAsync(int employeedId)
        {
            var expenses = await connection.Table<Expense>().Where(x => x.EmployeeId == employeedId).ToListAsync();
            return expenses;
        }

        public async Task InitializeDatabaseAsync()
        {
            connection = new SQLiteAsyncConnection("expenses.db");
            await connection.CreateTableAsync<Employee>();
            await connection.CreateTableAsync<Expense>();

            int result = await connection.Table<Employee>().CountAsync();
            if (result == 0)
            {
                await GenerateFakeData(numberOfEmployees, numberOfExpenses);
            }
        }

        private async Task GenerateFakeData(int numberOfEmployees, int numberOfExpenses)
        {
            for (int cont = 0; cont < numberOfEmployees; cont++)
            {
                var employee = new Faker<Employee>()
                    .RuleFor(x => x.FirstName, (f, u) => f.Name.FirstName())
                    .RuleFor(x => x.LastName, (f, u) => f.Name.LastName())
                    .RuleFor(x => x.Email, (f, u) => f.Internet.Email()).Generate();

                int employeeId = await connection.InsertAsync(employee);

                for (int contExpenses = 0; contExpenses < numberOfExpenses; contExpenses++)
                {
                    var expense = new Faker<Expense>()
                   .RuleFor(x => x.Description, (f, u) => f.Commerce.ProductName())
                   .RuleFor(x => x.Type, (f, u) => f.Finance.TransactionType())
                   .RuleFor(x => x.Cost, (f, u) => (double)f.Finance.Amount()).Generate();

                    expense.EmployeeId = cont + 1;

                    await connection.InsertAsync(expense);
                }
            }
        }
    }
}
