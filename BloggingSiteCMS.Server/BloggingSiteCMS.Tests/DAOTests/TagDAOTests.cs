using System.Linq.Expressions;

using BloggingSiteCMS.DAL;
using BloggingSiteCMS.DAL.DAO;
using BloggingSiteCMS.DAL.Domain;

using Moq;

using UpdateStatus = BloggingSiteCMS.DAL.Enums.UpdateStatus;

namespace BloggingSiteCMS.Tests.DAOTests
{
    public class TagDAOTests
    {
        private readonly TagDAO _dao;
        private readonly Mock<IRepository<Tag>> _mockRepo;

        public TagDAOTests()
        {
            _mockRepo = new();
            _dao = new TagDAO(_mockRepo.Object);
        }

        [Fact]
        public async Task Add_Should_Return_3()
        {
            // Arrange
            var tags = new List<Tag>
            {
                new() { Name = "Tag1" },
                new() { Name = "Tag2" },
                new() { Name = "Tag3" }
            };

            _mockRepo.Setup(repo => repo.Add(It.IsAny<Tag>())).ReturnsAsync((Tag tag) => tag);

            // Act
            await _dao.AddTagsAsync(tags);

            // Assert
            _mockRepo.Verify(repo => repo.Add(It.IsAny<Tag>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Update_Should_Return_Ok()
        {
            Tag tag = new() { Id = Guid.NewGuid().ToString(), Name = "Tag1" };

            _mockRepo.Setup(repo => repo.Update(It.IsAny<Tag>())).ReturnsAsync(UpdateStatus.Ok);

            UpdateStatus result = await _dao.UpdateTagAsync(tag);

            Assert.Equal(UpdateStatus.Ok, result);
            _mockRepo.Verify(repo => repo.Update(It.IsAny<Tag>()), Times.Once);
        }

        [Fact]
        public async Task Update_Should_Handle_Concurrency_Conflict()
        {
            // Can't mock this for some reason, nothing returns the right value, do this for now!

            // Arrange
            TagDAO dao1 = new();
            TagDAO dao2 = new();

            Tag tag = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Tag1"
            };

            await dao1.AddTagsAsync([tag]);

            Tag? testTag1 = await dao1.GetTagByNameAsync("Tag1");
            Tag? testTag2 = await dao2.GetTagByNameAsync("Tag1");

            testTag1!.Name = "Tag2";

            // Act
            if (await dao1.UpdateTagAsync(testTag1) == UpdateStatus.Ok)
            {
                testTag2!.Name = "Tag3";
                // Assert
                Assert.True(await dao2.UpdateTagAsync(testTag2) == UpdateStatus.Stale);
            }
            else
            {
                Assert.True(false);
            }

            _ = await dao1.DeleteTagAsync(testTag1!.Id!);
        }

        [Fact]
        public async Task GetByName_Should_Return_Tag()
        {
            // Arrange
            Tag tag = new() { Name = "Tag1" };

            _mockRepo.Setup(repo => repo.GetOne(It.IsAny<Expression<Func<Tag, bool>>>())).ReturnsAsync(tag);

            // Act
            Tag? result = await _dao.GetTagByNameAsync(tag.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tag, result);
            _mockRepo.Verify(repo => repo.GetOne(It.IsAny<Expression<Func<Tag, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetById_Should_Return_Tag()
        {
            // Arrange
            Tag tag = new() { Id = Guid.NewGuid().ToString() };

            _mockRepo.Setup(repo => repo.GetOne(It.IsAny<Expression<Func<Tag, bool>>>())).ReturnsAsync(tag);

            // Act
            Tag? result = await _dao.GetTagByNameAsync(tag.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tag, result);
            _mockRepo.Verify(repo => repo.GetOne(It.IsAny<Expression<Func<Tag, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Delete_1()
        {
            // Arrange
            Tag tag = new() { Id = Guid.NewGuid().ToString(), Name = "Tag1" };

            _mockRepo.Setup(repo => repo.Delete(tag.Id)).ReturnsAsync(1);

            // Act
            int result = await _dao.DeleteTagAsync(tag.Id);

            // Assert
            Assert.Equal(1, result);
            _mockRepo.Verify(repo => repo.Delete(tag.Id), Times.Once);
        }
    }
}
