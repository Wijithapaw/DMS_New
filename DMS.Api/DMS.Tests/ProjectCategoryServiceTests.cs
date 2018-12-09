using DMS.Data;
using DMS.Data.Entities;
using DMS.Domain.Dtos;
using DMS.Domain.Dtos.Project;
using DMS.Services;
using DMS.Utills.CustomExceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DMS.Tests
{
    public class ProjectCategoryServiceTests
    {
        public class Create
        {
            [Theory]
            [InlineData("Public Donation", "Donation for a public welfare project")]
            [InlineData("Religious Donation", "Donation for a religious matter")]
            public async Task WhenPassingCorrectData_SuccessfullyCreate(string shortDescription, string description)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectCategoryService(context);

                    var projectCategoryDto = new ProjectCategoryDto
                    {
                        ShortDescription = shortDescription,
                        Description = description
                    };

                    var id = await service.CreateAsync(projectCategoryDto);

                    var projectCategory = await service.GetAsync(id);

                    ValidateProjectCategory(projectCategory, null, shortDescription, description);
                }
            }
        }

        public class Delete
        {
            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            public async Task WhenDeletingExistingCategory_DeleteSuccessfully(int id)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectCategoryService(context);

                    var categoryBeforeDelete = service.GetAsync(id);
                    Assert.NotNull(categoryBeforeDelete);

                    await service.DeleteAsync(id);

                    var deletedCategory = await service.GetAsync(id);
                    Assert.Null(deletedCategory);
                }
            }

            [Theory]
            [InlineData(100)]
            [InlineData(200)]
            public async Task WhenDeletingNonExistingCategory_ThrowsException(int id)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectCategoryService(context);

                    await Assert.ThrowsAsync<RecordNotFoundException>(() => service.DeleteAsync(id));
                }
            }
        }

        public class Get
        {
            [Theory]
            [InlineData(1, "Educational", "Scholarship for primary, seconday or higher education.")]
            [InlineData(2, "Self Employment", "One-off donation for socially important matter.")]
            [InlineData(3, "Missalanious", "Donation for uncategorized purpose.")]
            public async Task WhenPassingValidId_ReturnsCategory(int id, string shortDescription, string description)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectCategoryService(context);

                    var category = await service.GetAsync(id);

                    ValidateProjectCategory(category, id, shortDescription, description);                    
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
                    var service = new ProjectCategoryService(context);

                    var category = await service.GetAsync(id);

                    Assert.Null(category);
                }
            }
        }

        public class GetAll
        {
            [Fact]
            public async Task WhenThereAreCategories_ReturnsAll()
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectCategoryService(context);
                    var categories = await service.GetAllAsync();
                    Assert.Equal(3, categories.Count);
                }
            }

            [Fact]
            public async Task WhenThereAreNoProjects_ReturnsNon()
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = new ProjectCategoryService(context);
                    var categories = await service.GetAllAsync();

                    Assert.Equal(0, categories.Count);
                }
            }
        }

        public class Update
        {
            [Theory]
            [InlineData(1, "Educational - Updated", "Scholarship for primary, seconday or higher education. - Updated")]
            [InlineData(2, "Self Employment", "Updated Description")]
            public async Task WhenUpdatingExistingCategory_UpdateSuccessfully(int id, string shortDescription, string description)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectCategoryService(context);

                    var categoryDto = new ProjectCategoryDto
                    {
                        Id = id,
                        ShortDescription = shortDescription,
                        Description = description
                    };

                    await service.UpdateAsync(categoryDto);

                    var updatedProject = await service.GetAsync(id);

                    ValidateProjectCategory(updatedProject, id, shortDescription, description);
                }
            }

            [Theory]
            [InlineData(100, "Educational - Updated", "Scholarship for primary, seconday or higher education. - Updated")]
            [InlineData(200, "Self Employment", "Updated Description")]
            public async Task WhenUpdatingNonExistingCategory_ThrowsException(int id, string shortDescription, string description)
            {
                var options = Helper.GetContextOptions();

                CreateTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new ProjectCategoryService(context);

                    var categoryDto = new ProjectCategoryDto
                    {
                        Id = id,
                        ShortDescription = shortDescription,
                        Description = description
                    };

                    await Assert.ThrowsAsync<RecordNotFoundException>(() => service.UpdateAsync(categoryDto));
                }
            }
        }

        #region

        private static void CreateTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();

                context.ProjectCategories.Add(CreateProjectCategory("Educational", "Scholarship for primary, seconday or higher education."));
                context.ProjectCategories.Add(CreateProjectCategory("Self Employment", "One-off donation for socially important matter."));
                context.ProjectCategories.Add(CreateProjectCategory("Missalanious", "Donation for uncategorized purpose."));

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

        private static void ValidateProjectCategory(ProjectCategoryDto projectCategoryDto, int? id, string shortDescription, string description )
        {
            if (id != null)
                Assert.Equal(id, projectCategoryDto.Id);

            Assert.Equal(shortDescription, projectCategoryDto.ShortDescription);
            Assert.Equal(description, projectCategoryDto.Description);
        }

        #endregion
    }
}
