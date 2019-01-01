using DMS.Data;
using DMS.Domain;
using DMS.Domain.CustomExceptions;
using DMS.Domain.Dtos.System;
using DMS.Domain.Entities;
using DMS.Domain.Entities.System;
using DMS.Services.System;
using DMS.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DMS.Tests
{
    public class LookupHeaderServiceTests
    {
        public class Get
        {
            [Theory]
            [InlineData(1)]
            [InlineData(3)]
            public async Task WhenPassingValidId_ReturnRecord(int id)
            {
                await DbHelper.ExecuteTestAsync(
                     async (IDataContext context) => { await SetupTestDataAsync(context); },
                     async (IDataContext context) =>
                     {
                         var service = new LookupHeaderService(context);

                         var header = await service.GetAsync(id);

                         Assert.NotNull(header);
                     });
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
                         var service = new LookupHeaderService(context);

                         await Assert.ThrowsAsync<RecordNotFoundException>(() => service.GetAsync(id));
                     });
            }

            [Theory]
            [InlineData(1, "GENDER", "Gender")]
            [InlineData(3, "PROJECT_CATEGORY", "Project Categories")]
            public async Task WhenPassingValidId_ReturnRecordWithCorrectData(int id, string code, string description)
            {
                await DbHelper.ExecuteTestAsync(
                     async (IDataContext context) => { await SetupTestDataAsync(context); },
                     async (IDataContext context) =>
                     {
                         var service = new LookupHeaderService(context);

                         var header = await service.GetAsync(id);

                         Assert.NotNull(header);
                         Assert.Equal(code, header.Code);
                         Assert.Equal(description, header.Description);
                     });
            }
        }

        public class GetAll
        {
            [Fact]
            public async Task WhenDataExists_ReturnAll()
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupHeaderService(context);

                        var headers = await service.GetAllAsync();

                        Assert.Equal(3, headers.Count);
                    });
            }
        }

        public class Create
        {
            [Theory]
            [InlineData("CODE_1", "Unit Test Data - 1")]
            [InlineData("CODE_2", "Unit Test Data - 2")]
            public async Task WhenPassingCorrectData_CreateSuccessfully(string code, string description)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupHeaderService(context);

                        var dto = new LookupHeaderDto
                        {
                            Code = code,
                            Description = description
                        };

                        var result = await service.CreateAsync(dto);

                        Assert.True(result.Id > 0);
                    });
            }

            [Theory]
            [InlineData("GENDER", "Unit Test Data - 1")]
            public async Task WhenPassingExistingCode_ThrowsException(string code, string description)
            {
                await DbHelper.ExecuteTestAsync(
                    async (IDataContext context) => { await SetupTestDataAsync(context); },
                    async (IDataContext context) =>
                    {
                        var service = new LookupHeaderService(context);

                        var dto = new LookupHeaderDto
                        {
                            Code = code,
                            Description = description
                        };

                        await Assert.ThrowsAsync<DbUpdateException>(() => service.CreateAsync(dto));
                    });
            }

            public class Update
            {
                [Theory]
                [InlineData(1, "GENDER", "Genders of People")]
                [InlineData(2, "SALUTATIONS_UPDATED", "Salutations updated")]
                [InlineData(3, "PROJECT_CATEGORY_UPDATED", "Project Categories")]
                public async Task WhenPassingCorrectData_UpdateSuccessfully(int id, string code, string description)
                {
                    await DbHelper.ExecuteTestAsync(
                        async (IDataContext context) => { await SetupTestDataAsync(context); },
                        async (IDataContext context) =>
                        {
                            var service = new LookupHeaderService(context);

                            var dto = new LookupHeaderDto
                            {
                                Id = id,
                                Code = code,
                                Description = description
                            };

                            await service.UpdateAsync(dto);
                        },
                        async (IDataContext context) =>
                        {
                            var service = new LookupHeaderService(context);

                            var header = await service.GetAsync(id);

                            Assert.NotNull(header);
                            Assert.Equal(code, header.Code);
                            Assert.Equal(description, header.Description);
                        });
                }

                [Theory]
                [InlineData(100, "GENDER", "Genders of People")]
                [InlineData(200, "SALUTATIONS_UPDATED", "Salutations updated")]
                public async Task WhenPassingInvalidId_ThrowsException(int id, string code, string description)
                {
                    await DbHelper.ExecuteTestAsync(
                        async (IDataContext context) => { await SetupTestDataAsync(context); },
                        async (IDataContext context) =>
                        {
                            var service = new LookupHeaderService(context);

                            var dto = new LookupHeaderDto
                            {
                                Id = id,
                                Code = code,
                                Description = description
                            };

                            await Assert.ThrowsAsync<RecordNotFoundException>(() => service.UpdateAsync(dto));
                        });
                }

                [Theory]
                [InlineData(1, "SALUTATIONS", "Genders of People")]
                public async Task WhenReplacingExistingCode_ThrowsException(int id, string code, string description)
                {
                    await DbHelper.ExecuteTestAsync(
                        async (IDataContext context) => { await SetupTestDataAsync(context); },
                        async (IDataContext context) =>
                        {
                            var service = new LookupHeaderService(context);

                            var dto = new LookupHeaderDto
                            {
                                Id = id,
                                Code = code,
                                Description = description
                            };

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
                            var service = new LookupHeaderService(context);
                            await service.DeleteAsync(id);
                        },
                        async (IDataContext context) =>
                        {
                            var service = new LookupHeaderService(context);
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
                            var service = new LookupHeaderService(context);
                            await Assert.ThrowsAsync<RecordNotFoundException>(() => service.DeleteAsync(id));
                        });
                }
            }
        }

        #region Suport Methods

        private async static Task SetupTestDataAsync(IDataContext context)
        {
            context.LookupHeaders.AddRange(GetLookupHeaders());

            await context.SaveChangesAsync();
        }

        private static LookupHeader[] GetLookupHeaders()
        {
            var data = new LookupHeader[]
                {
                    EntityHelper.CreateLookupHeader(1, "GENDER", "Gender"),
                    EntityHelper.CreateLookupHeader(2, "SALUTATIONS", "Saltations"),
                    EntityHelper.CreateLookupHeader(3, "PROJECT_CATEGORY", "Project Categories"),
                    EntityHelper.CreateLookupHeader(4, "CODE_1", "Deleted Code", true),
                    EntityHelper.CreateLookupHeader(5, "GENDER", "Deleted Code", true)
                };
            return data;
        }

        #endregion
    }
}
