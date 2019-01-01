using DMS.Data;
using DMS.Domain;
using DMS.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Tests
{
    public class DbHelper
    {
        /// <summary>
        /// This is a helper method to execute the 3 stages of a test method
        /// </summary>
        /// <param name="funcSetupTestDataAsync(IDataContext context)"> Set up test data inside this method. </param>
        /// <param name="funcTextExecutionAsync(IDataContext context)"> Place the main tst logic inside here. </param>
        /// <param name="funcValidationsAsync(IDataContext context)"> Use this method for additional validations against a fresh data context. This is optional. </param>
        /// <returns></returns>
        public static async Task ExecuteTestAsync(Func<IDataContext, Task> funcSetupTestDataAsync,
            Func<IDataContext, Task> funcTextExecutionAsync,
            Func<IDataContext, Task> funcValidationsAsync = null)
        {
            var requestContext = CreateRequestContext();

            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var options = GetSqliteContextOptions(connection);

                if (funcSetupTestDataAsync != null)
                {
                    using (IDataContext context = new DataContext(options, requestContext))
                    {
                        await funcSetupTestDataAsync(context);
                    }
                }

                using (IDataContext context = new DataContext(options, requestContext))
                {
                    await funcTextExecutionAsync(context);
                }

                if (funcValidationsAsync != null)
                {
                    using (IDataContext context = new DataContext(options, requestContext))
                    {
                        await funcValidationsAsync(context);
                    }
                }

                connection.Close();
            }
        }

        public static async Task ExecuteTestAsync3(Func<IDataContext, Task> funcSetupTestDataAsync,
            Func<IDataContext, Task> funcTextExecutionAsync,
            Func<IDataContext, Task> funcValidationsAsync = null)
        {
            var options = GetInMemoryContextOptions();

            var requestContext = CreateRequestContext();

            if (funcSetupTestDataAsync != null)
            {
                using (IDataContext context = new DataContext(options, requestContext))
                {
                    await funcSetupTestDataAsync(context);
                }
            }

            using (IDataContext context = new DataContext(options, requestContext))
            {
                await funcTextExecutionAsync(context);
            }

            if (funcValidationsAsync != null)
            {
                using (IDataContext context = new DataContext(options, requestContext))
                {
                    await funcValidationsAsync(context);
                }
            }

            using (var context = new DataContext(options))
            {
                await funcSetupTestDataAsync(context);
            }
        }

        //private static SqliteConnection GetSqliteConnection()
        //{
        //    var connection = new SqliteConnection("DataSource=:memory:");
        //    connection.Open();

        //    return connection;
        //}

        public static DbContextOptions<DataContext> GetSqliteContextOptions(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                   .UseSqlite(connection)
                   .Options;

            using (var context = new DataContext(options, CreateRequestContext()))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                CreateSystemUser(context).Wait();
            }

            return options;
        }


        #region ImMemoryDB

        public static DbContextOptions<DataContext> GetInMemoryContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                   .AddEntityFrameworkInMemoryDatabase()
                   .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .UseInternalServiceProvider(serviceProvider)
            .Options;

            return options;
        }

        #endregion


        private static async Task CreateSystemUser(IDataContext context)
        {
            var sysAdminUser = EntityHelper.CreateUser(1, "System", "Admin", "systemadmin@yopmail.com", true);

            context.Users.Add(sysAdminUser);

            await context.SaveChangesAsync();
        }

        private static IRequestContext CreateRequestContext()
        {
            var requestContet = new UnitTestRequestContext { UserId = 1 };
            return requestContet;
        }

        private class UnitTestRequestContext : IRequestContext
        {
            public int UserId { get; set; }
        }
    }
}
