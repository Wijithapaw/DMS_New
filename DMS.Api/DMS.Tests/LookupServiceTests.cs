using DMS.Data;
using DMS.Domain;
using DMS.Domain.CustomExceptions;
using DMS.Domain.Dtos.System;
using DMS.Domain.Entities;
using DMS.Domain.Entities.System;
using DMS.Services.System;
using DMS.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DMS.Tests
{
    public class LookupServiceTests
    {
        public class Get
        {
            [Theory]
            [InlineData(1)]
            [InlineData(3)]
            public async Task WhenPassingValidId_ReturnRecord(int id)
            {
                using (var connection = new SqliteConnection("DataSource=:memory:"))
                {
                    connection.Open();

                    var options = DbHelper.GetSqliteContextOptions(connection);


                    using (IDataContext context = new DataContext(options))
                    {
                        await SetupTestDataAsync(context);
                    }

                    using (IDataContext context = new DataContext(options))
                    {
                        var x = await context.Lookups.ToListAsync();

                        var service = new LookupService(context);

                        var lookup = await service.GetAsync(id);

                        Assert.NotNull(lookup);
                    }
                }

                //await DbHelper.ExecuteTestAsync(
                //     async (IDataContext context) => { await SetupTestDataAsync(context); },
                //     async (IDataContext context) =>
                //     {
                //         var x = await context.Lookups.ToListAsync();

                //         var service = new LookupService(context);

                //         var lookup = await service.GetAsync(id);

                //         Assert.NotNull(lookup);
                //     });
            }

            [Theory]
            [InlineData(100)]
            [InlineData(300)]
            public async Task WhenPassingInvalidId_ThrowsException(int id)
            {
                await DbHelper.ExecuteTestAsync(
                     async (IDataContext context) => { await SetupTestDataAsync(context); },
                     async (IDataContext context) =>
                     {
                         var service = new LookupService(context);

                         await Assert.ThrowsAsync<RecordNotFoundException>(() => service.GetAsync(id));
                     });
            }

            [Theory]
            [InlineData(10)]
            [InlineData(11)]
            public async Task WhenPassingDeletedRecordIds_ThrowsException(int id)
            {
                await DbHelper.ExecuteTestAsync(
                     async (IDataContext context) => { await SetupTestDataAsync(context); },
                     async (IDataContext context) =>
                     {
                         var service = new LookupService(context);

                         await Assert.ThrowsAsync<RecordNotFoundException>(() => service.GetAsync(id));
                     });
            }

            [Theory]
            [InlineData(1, 1, "MALE", "Male", 1)]
            [InlineData(2, 1, "FEMALE", "Female", 2)]
            [InlineData(3, 2, "MR", "Mr.", 1)]
            public async Task WhenPassingValidId_ReturnRecordWithCorrectData(int id, int headerId, string code, string description, int sortOrder)
            {
                await DbHelper.ExecuteTestAsync(
                     async (IDataContext context) => { await SetupTestDataAsync(context); },
                     async (IDataContext context) =>
                     {
                         var service = new LookupService(context);

                         var lookup = await service.GetAsync(id);

                         Assert.NotNull(lookup);
                         Assert.Equal(code, lookup.Code);
                         Assert.Equal(description, lookup.Description);
                         Assert.Equal(headerId, lookup.HeaderId);
                         Assert.Equal(sortOrder, lookup.SortOrder);
                     });

            }
        }

        public class GetAllByHeaderId
        {
            [Theory]
            [InlineData(1, 2)]
            [InlineData(2, 6)]
            [InlineData(3, 0)]
            public async Task WhenDataExists_ReturnAllBelongsToHeader(int headerId, int count)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);

                        var lookups = await service.GetAllByHeaderIdAsync(headerId);

                        Assert.Equal(count, lookups.Count);
                    });
            }
        }

        public class GetAllByHeaderCode
        {
            [Theory]
            [InlineData("GENDER", 2)]
            [InlineData("SALUTATION", 6)]
            [InlineData("PROJECT_CATEGORY", 0)]
            public async Task WhenDataExists_ReturnAllBelongsToHeader(string headerCode, int count)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);

                        var lookups = await service.GetAllByHeaderCodeAsync(headerCode);

                        Assert.Equal(count, lookups.Count);
                    });
            }
        }

        public class Create
        {
            [Theory]
            [InlineData(1, "MR", "Mr. - Unit Test", 3)]
            [InlineData(2, "GEN", "Gen.", 10)]
            [InlineData(2, "MAJOR", "Major.", 11)]
            [InlineData(3, "EDUCATIONAL", "Educational Project", 1)]
            [InlineData(3, "SELF_EMPLOYMENT", "Self Employment", 2)]
            public async Task WhenPassingCorrectData_CreateSuccessfully(int headerId, string code, string description, int sortOrder)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);

                        var dto = DtoHelper.CreateLookupDto(headerId, code, description, sortOrder);

                        var x = await service.GetAllByHeaderIdAsync(headerId);

                        var result = await service.CreateAsync(dto);

                        Assert.True(result.Id > 0);
                    });
            }

            [Theory]
            [InlineData(2, "MR", "Mr.", 1)]
            public async Task WhenPassingExistingCode_ThrowsException(int headerId, string code, string description, int sortOrder)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);

                        var dto = DtoHelper.CreateLookupDto(headerId, code, description, sortOrder);

                        await Assert.ThrowsAsync<DbUpdateException>(() => service.CreateAsync(dto));
                    });
            }
        }

        public class Update
        {
            [Theory]
            [InlineData(1, 1, "MALE", "Boys", 1)]
            [InlineData(2, 1, "FEMALE", "Female", 0)]
            public async Task WhenPassingCorrectData_UpdateSuccessfully(int id, int headerId, string code, string description, int sortOrder)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);

                        var dto = DtoHelper.CreateLookupDto(headerId, code, description, sortOrder, id);

                        await service.UpdateAsync(dto);
                    },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);

                        var lookup = await service.GetAsync(id);

                        Assert.NotNull(lookup);
                        Assert.Equal(code, lookup.Code);
                        Assert.Equal(description, lookup.Description);
                        Assert.Equal(sortOrder, lookup.SortOrder);
                        Assert.Equal(headerId, lookup.HeaderId);
                    });
            }

            [Theory]
            [InlineData(100, 1, "MALE", "Boys", 1)]
            [InlineData(200, 1, "FEMALE", "Female", 0)]
            public async Task WhenPassingInvalidId_ThrowsException(int id, int headerId, string code, string description, int sortOrder)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);

                        var dto = DtoHelper.CreateLookupDto(headerId, code, description, sortOrder, id);

                        await Assert.ThrowsAsync<RecordNotFoundException>(() => service.UpdateAsync(dto));
                    });
            }

            [Theory]
            [InlineData(4, 2, "MR", "Mrs.", 2)]
            public async Task WhenReplacingExistingCode_ThrowsException(int id, int headerId, string code, string description, int sortOrder)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);

                        var dto = DtoHelper.CreateLookupDto(headerId, code, description, sortOrder, id);

                        await Assert.ThrowsAsync<DbUpdateException>(() => service.UpdateAsync(dto));
                    });
            }
        }

        public class Delete
        {
            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            public async Task WhenPassingValidId_DeleteSuccessfully(int id)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);
                        await service.DeleteAsync(id);
                    },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);
                        await Assert.ThrowsAsync<RecordNotFoundException>(() => service.GetAsync(id));
                    });
            }

            [Theory]
            [InlineData(100)]
            public async Task WhenPassingInvalidId_ThrowsException(int id)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupService(context);
                        await Assert.ThrowsAsync<RecordNotFoundException>(() => service.DeleteAsync(id));
                    });
            }
        }

        #region Suport Methods

        private async static Task SetupTestDataAsync(IDataContext context)
        {
            context.LookupHeaders.AddRange(GetLookupHeaders());
            await context.SaveChangesAsync();

            context.Lookups.AddRange(GetLookups());
            await context.SaveChangesAsync();
        }

        private static LookupHeader[] GetLookupHeaders()
        {
            var data = new LookupHeader[]
                {
                    EntityHelper.CreateLookupHeader(1, "GENDER", "Gender"),
                    EntityHelper.CreateLookupHeader(2, "SALUTATION", "Saltations"),
                    EntityHelper.CreateLookupHeader(3, "PROJECT_CATEGORY", "Project Categories"),
                    EntityHelper.CreateLookupHeader(4, "CODE_1", "Deleted Code", true),
                    EntityHelper.CreateLookupHeader(5, "GENDER", "Deleted Code", true),
                };
            return data;
        }

        private static Lookup[] GetLookups()
        {
            var data = new Lookup[]
                {
                    EntityHelper.CreateLookup(1, 1, "MALE", "Male", 1),
                    EntityHelper.CreateLookup(2, 1, "FEMALE", "Female", 2),

                    EntityHelper.CreateLookup(3, 2, "MR", "Mr.", 1),
                    EntityHelper.CreateLookup(4, 2, "MRS", "Mrs.", 2),
                    EntityHelper.CreateLookup(5, 2, "MISS", "Miss", 3),
                    EntityHelper.CreateLookup(6, 2, "DR", "Dr.", 4),
                    EntityHelper.CreateLookup(7, 2, "PROF", "Prof.", 5),
                    EntityHelper.CreateLookup(8, 2, "REV", "Rev.", 6),
                    EntityHelper.CreateLookup(9, 2, "DR", "Dr1.", 4, true),
                    EntityHelper.CreateLookup(10, 2, "DR", "Dr2.", 4, true),
                    EntityHelper.CreateLookup(11, 2, "Rev", "Rev1.", 6, true),
                };

            return data;
        }

        #endregion
    }
}
