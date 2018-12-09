using DMS.Data;
using DMS.Data.Entities;
using DMS.Domain.Dtos;
using DMS.Domain.Dtos.Project;
using DMS.Services;
using DMS.Utills.CustomExceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                new object[] { "Sholaship for Supun", "As a aid for university education", 1, "Educational", new DateTime(2017,3,1), new DateTime(2018, 12, 31)},
                new object[] { "Sholaship for Amal", "Start a workshop", 2, "Self Employment",  new DateTime(2017,2,1), new DateTime(2017, 2, 28) },
             };


            [Theory]            
            [MemberData(nameof(DataCreate))]
            public async Task WhenPassingCorrectData_SuccessfullyCreate(string title, string description, int categoryId, string category, DateTime startDate, DateTime endDate)
            {
                DateTime d1 = new DateTime(2011, 1, 1);
                DateTime d2 = DateTime.Now;
                DateTime d3 = DateTime.UtcNow;



                DateTime d4 = DateTime.Parse("2011-1-1");

                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);

                    var projectDto = new ProjectDto
                    {
                        Title = title,
                        Description = description,
                        ProjectCategoryId = categoryId,
                        StartDate = startDate,
                        EndDate = endDate
                    };

                    var id = await service.CreateAsync(projectDto);

                    var project = await service.GetAsync(id);

                    ValidateProject(project, null, title, description, category, startDate, endDate);
                }
            }          
        }

        public class Delete
        {
            [Fact]
            public async Task WhenDeletingExistingProject_DeleteSuccessfully()
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);

                    await service.DeleteAsync(1);

                    var deletedProject = await service.GetAsync(1);

                    Assert.Null(deletedProject);
                }
            }

            [Fact]
            public async Task WhenDeletingNonExistingProject_ThrowsException()
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);

                    await Assert.ThrowsAsync<RecordNotFoundException>(() => service.DeleteAsync(100));                  
                }
            }
        }

        public class Get
        {
            public static IEnumerable<object[]> Data_Get =>
            new List<object[]>
            {
                new object[] { 1, "Scholaship for Asitha", "For higher education", "Educational", new DateTime(2017,1,1), new DateTime(2017,12,31)},
                new object[] { 2, "Scholaship for Supun", "For O/L education", "Educational", new DateTime(2017,2,1), new DateTime(2018,3,31) },
                new object[] { 3, "Scholaship for Viraj", "Aid for self employment", "Self Employment", new DateTime(2017, 3, 1), null }
            };

            [Theory]
            [MemberData(nameof(Data_Get))]
            public async Task WhenPassingValidIds_ReturnsProject(int id, string title, string description, string category, DateTime startDate, DateTime endDate)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);

                    var project = await service.GetAsync(id);

                    Assert.NotNull(project);
                    Assert.Equal(title, project.Title);
                    Assert.Equal(description, project.Description);
                    Assert.Equal(category, project.ProjectCategory);
                    Assert.Equal(startDate, project.StartDate);

                    if (endDate == DateTime.MinValue)
                        Assert.Null(project.EndDate);
                }
            }

            [Theory]
            [InlineData(10)]
            [InlineData(100)]
            [InlineData(101)]
            public async Task WhenPassingInvalidIds_ReturnsNull(int id)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);

                    var project = await service.GetAsync(id);

                    Assert.Null(project);
                }
            }
        }

        public class GetAll
        {
            [Fact]
            public async Task WhenThereAreProjects_ReturnsAll()
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);
                    var projects = await service.GetAllAsync();
                    Assert.Equal(3, projects.Count);
                }
            }

            [Fact]
            public async Task WhenThereAreNoProjects_ReturnsNon()
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);
                    var projects = await service.GetAllAsync();

                    Assert.Equal(0, projects.Count);
                }
            }

            [Theory]
            [InlineData("Educational", 2)]
            [InlineData("Self Employment", 1)]
            [InlineData("Missalanious", 0)]
            public async Task WhenThereProjects_ReturnsAllInSameCategory(string category, int count)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);
                    var projects = await service.GetAllAsync(category);
                    Assert.Equal(count, projects.Count);
                }
            }

            [Theory]
            [InlineData("Educational", 0)]
            [InlineData("Self Employment", 0)]
            [InlineData("Missalanious", 0)]
            public async Task WhenThereAreNoProjects_ReturnsNonWhenPassingCategory(string category, int count)
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);
                    var projects = await service.GetAllAsync(category);
                    Assert.Equal(count, projects.Count);
                }
            }
        }

        public class Update
        {
            public static IEnumerable<object[]> Data_Update =>
            new List<object[]>
            {
                new object[] { 1, "Scholaship for Asitha - Updated", "For higher education - Updated", 1, "Educational", new DateTime(2017,1,1), new DateTime(2017,12,31)},
                new object[] { 2, "Scholaship for Supun", "Start Workshop", 2, "Self Employment", new DateTime(2017,2,1), new DateTime(2018,3,31) },
                new object[] { 3, "Scholaship for Kasun - Updated", "Buy a sewing machine", 3, "Missalanious", new DateTime(2017, 5, 1), new DateTime(2019, 5, 1) }
            };

            [Theory]
            [MemberData(nameof(Data_Update))]
            public async Task WhenUpdatingExistingProject_UpdateSuccessfully(int id, string title, string description, int categoryId, string category, DateTime startDate, DateTime endDate)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectsService(context);

                    var projectDto = new ProjectDto
                    {
                        Id = id,
                        Title = title,
                        Description = description,
                        ProjectCategoryId = categoryId,
                        StartDate = startDate,
                        EndDate = endDate
                    };

                    await service.UpdateAsync(projectDto);

                    var updatedProject = await service.GetAsync(id);

                    ValidateProject(updatedProject, id, title, description, category, startDate, endDate);
                }
            }

            [Fact]
            public async Task WhenUpdatingNonExistingProject_ThrowsException()
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
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
                }
            }
        }

        #region Private support methods

        private static void CreateTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();

                context.ProjectCategories.Add(CreateProjectCategory("Educational", "Scholarship for primary, seconday or higher education."));
                context.ProjectCategories.Add(CreateProjectCategory("Self Employment", "One-off donation for socially important matter."));
                context.ProjectCategories.Add(CreateProjectCategory("Missalanious", "Donation for uncategorized purpose."));

                context.Projects.Add(CreateProject("Scholaship for Asitha", "For higher education", 1, new DateTime(2017, 1, 1), new DateTime(2017, 12, 31)));
                context.Projects.Add(CreateProject("Scholaship for Supun", "For O/L education", 1, new DateTime(2017, 2, 1), new DateTime(2018, 3, 31)));
                context.Projects.Add(CreateProject("Scholaship for Viraj", "Aid for self employment", 2, new DateTime(2017, 3, 1), null));

                context.SaveChanges();
            }
        }

        private static ProjectCategory CreateProjectCategory(string shortDesc, string description)
        {
            return new ProjectCategory
            {
                Description = description,
                Title = shortDesc,
            };
        }

        private static Project CreateProject(string title, string description, int categoryId, DateTime startDate, DateTime? endDate)
        {
            return new Project
            {
                Title = title,
                Description = description,
                ProjectCategoryId = categoryId,
                StartDate = startDate,
                EndDate = endDate,
            };
        }


        private static void ValidateProject(ProjectDto projectDto, int? id, string title, string description, string category, DateTime startDate, DateTime endDate)
        {
            Assert.NotNull(projectDto);

            if (id != null)
                Assert.Equal(id, projectDto.Id);

            Assert.Equal(title, projectDto.Title);
            Assert.Equal(description, projectDto.Description);
            Assert.Equal(category, projectDto.ProjectCategory);
            Assert.Equal(startDate, projectDto.StartDate);

            if (endDate == DateTime.MinValue)
                Assert.Null(projectDto.EndDate);
            else
                Assert.Equal(endDate, projectDto.EndDate);
        }

        #endregion
    }
}
