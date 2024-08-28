using System.Linq.Expressions;

using BloggingSiteCMS.DAL.DAO;
using BloggingSiteCMS.DAL.Domain;
using BloggingSiteCMS.DAL;

using Moq;

using UpdateStatus = BloggingSiteCMS.DAL.Enums.UpdateStatus;

namespace BloggingSiteCMS.Tests.DAOTests
{
    public class PostDAOTests
    {
        private readonly PostDAO _dao;
        private readonly Mock<IRepository<Post>> _mockRepo;

        public PostDAOTests()
        {
            _mockRepo = new();
            _dao = new PostDAO(_mockRepo.Object);
        }

        [Fact]
        public async Task Add_Should_Return_1()
        {
            // Arrange
            Post post = new Post()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test Post",
                Author = new AppUser(),
                Content = "Testing!",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                PublishedDate = DateTime.Now,
            };

            _mockRepo.Setup(repo => repo.Add(It.IsAny<Post>())).ReturnsAsync((Post post) => post);

            // Act
            await _dao.AddPostAsync(post);

            // Assert
            _mockRepo.Verify(repo => repo.Add(It.IsAny<Post>()), Times.Exactly(1));
        }

        [Fact]
        public async Task Update_Should_Return_Ok()
        {
            // Arrange
            Post post = new Post()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test Post",
                Author = new AppUser(),
                Content = "Testing!",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                PublishedDate = DateTime.Now,
            };

            _mockRepo.Setup(repo => repo.Update(It.IsAny<Post>())).ReturnsAsync(UpdateStatus.Ok);

            UpdateStatus result = await _dao.UpdatePostAsync(post);

            Assert.Equal(UpdateStatus.Ok, result);
            _mockRepo.Verify(repo => repo.Update(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task Update_Should_Handle_Concurrency_Conflict()
        {
            // Can't mock this for some reason, nothing returns the right value, do this for now!

            // Arrange
            PostDAO dao1 = new();
            PostDAO dao2 = new();

            Post post = new Post()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test Post",
                Author = new AppUser(),
                Content = "Testing!",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                PublishedDate = DateTime.Now,
            };

            await dao1.AddPostAsync(post);

            Post? testPost1 = await dao1.GetPostAsyncByIdAsync(post.Id);
            Post? testPost2 = await dao2.GetPostAsyncByIdAsync(post.Id);

            testPost1!.Title = "Test Post 1";

            // Act
            if (await dao1.UpdatePostAsync(testPost1) == UpdateStatus.Ok)
            {
                testPost2!.Title = "Test Post 2";
                // Assert
                Assert.True(await dao2.UpdatePostAsync(testPost2) == UpdateStatus.Stale);
            }
            else
            {
                Assert.True(false);
            }

            _ = await dao1.DeletePostAsync(testPost1!.Id!);
        }

        [Fact]
        public async Task Get_Should_Return_Post()
        {
            // Arrange
            Post post = new Post()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test Post",
                Author = new AppUser(),
                Content = "Testing!",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                PublishedDate = DateTime.Now,
            };

            _mockRepo.Setup(repo => repo.GetOne(It.IsAny<Expression<Func<Post, bool>>>())).ReturnsAsync(post);

            // Act
            Post? result = await _dao.GetPostAsyncByIdAsync(post.Id);

            // Assert
            Assert.Equal(post, result);
            _mockRepo.Verify(repo => repo.GetOne(It.IsAny<Expression<Func<Post, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Delete_1()
        {
            // Arrange
            Post post = new Post()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test Post",
                Author = new AppUser(),
                Content = "Testing!",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                PublishedDate = DateTime.Now,
            };

            _mockRepo.Setup(repo => repo.Delete(post.Id)).ReturnsAsync(1);

            // Act
            int result = await _dao.DeletePostAsync(post.Id);

            // Assert
            Assert.Equal(1, result);
            _mockRepo.Verify(repo => repo.Delete(post.Id), Times.Once);
        }
    }
}
