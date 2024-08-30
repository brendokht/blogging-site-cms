using System.Linq.Expressions;

using BloggingSiteCMS.DAL;
using BloggingSiteCMS.DAL.DAO;
using BloggingSiteCMS.DAL.Domain;

using Moq;

using UpdateStatus = BloggingSiteCMS.DAL.Enums.UpdateStatus;

namespace BloggingSiteCMS.Tests.DAOTests
{
    public class CategoryDAOTests
    {
        private readonly CategoryDAO _dao;
        private readonly Mock<IRepository<Category>> _mockRepo;

        public CategoryDAOTests()
        {
            _mockRepo = new();
            _dao = new CategoryDAO(_mockRepo.Object);
        }

        [Fact]
        public async Task Add_Should_Return_1()
        {
            // Arrange
            var categories = new List<Category>
            {
                new() { Name = "JavaScript" },
                new() { Name = "C#" },
                new() { Name = "Recat" }
            };

            _mockRepo.Setup(repo => repo.Add(It.IsAny<Category>())).ReturnsAsync((Category category) => category);

            // Act
            await _dao.AddCategoriesAsync(categories);

            // Assert
            _mockRepo.Verify(repo => repo.Add(It.IsAny<Category>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Update_Should_Return_Ok()
        {
            Category category = new() { Id = Guid.NewGuid().ToString(), Name = "Category1" };

            _mockRepo.Setup(repo => repo.Update(It.IsAny<Category>())).ReturnsAsync(UpdateStatus.Ok);

            UpdateStatus result = await _dao.UpdateCategoryAsync(category);

            Assert.Equal(UpdateStatus.Ok, result);
            _mockRepo.Verify(repo => repo.Update(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task Update_Should_Handle_Concurrency_Conflict()
        {
            // Can't mock this for some reason, nothing returns the right value, do this for now!

            // Arrange
            CategoryDAO dao1 = new();
            CategoryDAO dao2 = new();

            Category category = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Category1",
                Description = "Category1 Description"
            };

            await dao1.AddCategoriesAsync([category]);

            Category? testCategory1 = await dao1.GetCategoryByNameAsync("Category1");
            Category? testCategory2 = await dao2.GetCategoryByNameAsync("Category1");

            testCategory1!.Name = "Category2";

            // Act
            if (await dao1.UpdateCategoryAsync(testCategory1) == UpdateStatus.Ok)
            {
                testCategory2!.Name = "Category3";
                // Assert
                Assert.True(await dao2.UpdateCategoryAsync(testCategory2) == UpdateStatus.Stale);
            }
            else
            {
                Assert.True(false);
            }

            _ = await dao1.DeleteCategoryAsync(testCategory1!.Id!);
        }

        [Fact]
        public async Task GetByName_Should_Return_Category()
        {
            // Arrange
            Category category = new() { Name = "Category1" };

            _mockRepo.Setup(repo => repo.GetOne(It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(category);

            // Act
            Category? result = await _dao.GetCategoryByNameAsync(category.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category, result);
            _mockRepo.Verify(repo => repo.GetOne(It.IsAny<Expression<Func<Category, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetById_Should_Return_Category()
        {
            // Arrange
            Category category = new() { Id = Guid.NewGuid().ToString() };

            _mockRepo.Setup(repo => repo.GetOne(It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(category);

            // Act
            Category? result = await _dao.GetCategoryByNameAsync(category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category, result);
            _mockRepo.Verify(repo => repo.GetOne(It.IsAny<Expression<Func<Category, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Delete_1()
        {
            // Arrange
            Category category = new() { Id = Guid.NewGuid().ToString(), Name = "Category1" };

            _mockRepo.Setup(repo => repo.Delete(category.Id)).ReturnsAsync(1);

            // Act
            int result = await _dao.DeleteCategoryAsync(category.Id);

            // Assert
            Assert.Equal(1, result);
            _mockRepo.Verify(repo => repo.Delete(category.Id), Times.Once);
        }
    }
}
