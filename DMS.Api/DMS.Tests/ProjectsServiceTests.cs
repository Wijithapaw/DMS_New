using DMS.Data;
using DMS.Domain;
using DMS.Domain.CustomExceptions;
using DMS.Domain.Dtos.Project;
using DMS.Domain.Entities;
using DMS.Services;
using DMS.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DMS.Tests
{
    public class ProjectsServiceTests
    {
        public class Create
        {
            public static IEnumerable<object[]> DataCreate =>
             new List<object[]>
             {
                new object[] { "Sholaship for Supun", "As a aid for university education", 1, new DateTime(2017,3,1), new DateTime(2018, 12, 31)},
                new object[] { "Sholaship for Amal", "Start a workshop", 2,  new DateTime(2017,2,1), new DateTime(2017, 2, 28) },
             };

            [Theory]
            [MemberData(nameof(DataCreate))]
            public async Task WhenPassingCorrectData_SuccessfullyCreate(string title, string description, int categoryId, DateTime startDate, DateTime endDate)
            {
                await DbHelper.ExecuteTestAsync(
                       async (IDataContext context) => { await SetupTestDataAsync(context); },
                       async (IDataContext context) =>
                       {
                           var service = new ProjectsService(context);

                           var projectDto = new ProjectDto
                           {
                               Title = title,
                               Description = description,
                               ProjectCategoryId = categoryId,
                               OwnerId = 1,
                               StartDate = startDate,
                               EndDate = endDate
                           };

                           var id = await service.CreateAsync(projectDto);

                           var project = await service.GetAsync(id);

                           ValidateProject(project, null, title, description, startDate, endDate);
                       });
            }        
        }

        public class Delete
        {
            [Fact]
            public async Task WhenDeletingExistingProject_DeleteSuccessfully()
            {
                await DbHelper.ExecuteTestAsync(
                      async (IDataContext context) => { await SetupTestDataAsync(context); },
                      async (IDataContext context) =>
                      {
                          var service = new ProjectsService(context);

                          await service.DeleteAsync(1);
                      },
                      async (IDataContext context) =>
                      {
                          var service = new ProjectsService(context);

                          var deletedProject = await service.GetAsync(1);

                          Assert.Null(deletedProject);
                      });
            }

            [Fact]
            public async Task WhenDeletingNonExistingProject_ThrowsException()
            {
                await DbHelper.ExecuteTestAsync(
                     async (IDataContext context) => { await SetupTestDataAsync(context); },
                     async (IDataContext context) =>
                     {
                         var service = new ProjectsService(context);

                         await Assert.ThrowsAsync<RecordNotFoundException>(() => service.DeleteAsync(100));
                     });
            }
        }

        public class Get
        {
            public static IEnumerable<object[]> Data_Get =>
            new List<object[]>
            {
                new object[] { 1, "Scholaship for Asitha", "For higher education", new DateTime(2017,1,1), new DateTime(2017,12,31)},
                new object[] { 2, "Scholaship for Supun", "For O/L education", new DateTime(2017,2,1), new DateTime(2018,3,31) },
                new object[] { 3, "Scholaship for Viraj", "Aid for self employment", new DateTime(2017, 3, 1), null }
            };

            [Theory]
            [MemberData(nameof(Data_Get))]
            public async Task WhenPassingValidIds_ReturnsProject(int id, string title, string description, DateTime startDate, DateTime endDate)
            {
                await DbHelper.ExecuteTestAsync(
                     async (IDataContext context) => { await SetupTestDataAsync(context); },
                     async (IDataContext context) =>
                     {
                         var service = new ProjectsService(context);

                         var x = await context.Projects.ToListAsync();

                         var project = await service.GetAsync(id);

                         Assert.NotNull(project);
                         Assert.Equal(title, project.Title);
                         Assert.Equal(description, project.Description);
                         Assert.Equal(startDate, project.StartDate);

                         if (endDate == DateTime.MinValue)
                             Assert.Null(project.EndDate);
                     });
            }

            [Theory]
            [InlineData(10)]
            [InlineData(100)]
            [InlineData(101)]
            public async Task WhenPassingInvalidIds_ReturnsNull(int id)
            {
                await DbHelper.ExecuteTestAsync(
                     async (IDataContext context) => { await SetupTestDataAsync(context); },
                     async (IDataContext context) =>
                     {
                         var service = new ProjectsService(context);

                         var project = await service.GetAsync(id);

                         Assert.Null(project);
                     });
            }
        }

        public class GetAll
        {
            [Fact]
            public async Task WhenThereAreProjects_ReturnsAll()
            {
                await DbHelper.ExecuteTestAsync(
                     async (IDataContext context) => { await SetupTestDataAsync(context); },
                     async (IDataContext context) =>
                     {
                         var service = new ProjectsService(context);
                         var projects = await service.GetAllAsync();
                         Assert.Equal(3, projects.Count);
                     });
            }

            [Fact]
            public async Task WhenThereAreNoProjects_ReturnsNon()
            {
                await DbHelper.ExecuteTestAsync(
                    null,
                    async (IDataContext context) =>
                    {
                        var service = new ProjectsService(context);
                        var projects = await service.GetAllAsync();

                        Assert.Equal(0, projects.Count);
                    });
            }
        }

        public class Update
        {
            public static IEnumerable<object[]> Data_Update =>
            new List<object[]>
            {
                new object[] { 1, "Scholaship for Asitha - Updated", "For higher education - Updated", new DateTime(2017,1,1), new DateTime(2017,12,31)},
                new object[] { 2, "Scholaship for Supun", "Start Workshop", new DateTime(2017,2,1), new DateTime(2018,3,31) },
                new object[] { 3, "Scholaship for Kasun - Updated", "Buy a sewing machine", new DateTime(2017, 5, 1), new DateTime(2019, 5, 1) }
            };

            [Theory]
            [MemberData(nameof(Data_Update))]
            public async Task WhenUpdatingExistingProject_UpdateSuccessfully(int id, string title, string description, DateTime startDate, DateTime endDate)
            {
                await DbHelper.ExecuteTestAsync(
                   async (IDataContext context) => { await SetupTestDataAsync(context); },
                   async (IDataContext context) =>
                   {
                       var service = new ProjectsService(context);

                       var projectDto = new ProjectDto
                       {
                           Id = id,
                           Title = title,
                           Description = description,
                           StartDate = startDate,
                           EndDate = endDate
                       };

                       await service.UpdateAsync(projectDto);
                   },
                   async (IDataContext context) =>
                   {
                       var service = new ProjectsService(context);

                       var updatedProject = await service.GetAsync(id);

                       ValidateProject(updatedProject, id, title, description, startDate, endDate);
                   });
            }

            [Fact]
            public async Task WhenUpdatingNonExistingProject_ThrowsException()
            {
                await DbHelper.ExecuteTestAsync(
                       async (IDataContext context) => { await SetupTestDataAsync(context); },
                       async (IDataContext context) =>
                       {
                           var service = new ProjectsService(context);

                           var projectDto = new ProjectDto
                           {
                               Id = 100,
                               Title = "New Project",
                               Description = "This shoudl fail",
                               ProjectCategoryId = 1,
                               StartDate = new DateTime(2017, 3, 1),
                               EndDate = new DateTime(2018, 3, 1)
                           };

                           await Assert.ThrowsAsync<RecordNotFoundException>(() => service.UpdateAsync(projectDto));
                       });
            }
        }

        #region Private support methods

        private async static Task SetupTestDataAsync(IDataContext context)
        {
            context.Projects.AddRange(GetProjects());

            await context.SaveChangesAsync();
        }

        private static Project[] GetProjects()
        {
            var projects = new Project[]
                {
                    EntityHelper.CreateProject(1, "Scholaship for Asitha", "For higher education", 1,  new DateTime(2017, 1, 1), new DateTime(2017, 12, 31)),
                    EntityHelper.CreateProject(2, "Scholaship for Supun", "For O/L education", 1, new DateTime(2017, 2, 1), new DateTime(2018, 3, 31)),
                    EntityHelper.CreateProject(3, "Scholaship for Viraj", "Aid for self employment", 1, new DateTime(2017, 3, 1), null)
                };
            return projects;
        }

        private static void ValidateProject(ProjectDto projectDto, int? id, string title, string description, DateTime startDate, DateTime endDate)
        {
            Assert.NotNull(projectDto);

            if (id != null)
                Assert.Equal(id, projectDto.Id);

            Assert.Equal(title, projectDto.Title);
            Assert.Equal(description, projectDto.Description);
            Assert.Equal(startDate, projectDto.StartDate);

            if (endDate == DateTime.MinValue)
                Assert.Null(projectDto.EndDate);
            else
                Assert.Equal(endDate, projectDto.EndDate);
        }

        #endregion
    }
}
