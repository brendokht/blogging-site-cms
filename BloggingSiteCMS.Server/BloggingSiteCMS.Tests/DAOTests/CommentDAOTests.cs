using System.Linq.Expressions;

using BloggingSiteCMS.DAL;
using BloggingSiteCMS.DAL.DAO;
using BloggingSiteCMS.DAL.Domain;

using Microsoft.AspNetCore.Identity;

using Moq;

using UpdateStatus = BloggingSiteCMS.DAL.Enums.UpdateStatus;

namespace BloggingSiteCMS.Tests.DAOTests
{
    public class CommentDAOTests
    {
        private readonly CommentDAO _dao;
        private readonly Mock<IRepository<Comment>> _mockRepo;
        private readonly CMSContext _context;

        public CommentDAOTests()
        {
            _mockRepo = new();
            _dao = new CommentDAO(_mockRepo.Object);
            _context = new CMSContext();
        }

        [Fact]
        public async Task Add_Should_Return_1()
        {
            // Arrange
            Comment comment = new()
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Wow!",
                Author = It.IsAny<AppUser>(),
                Post = It.IsAny<Post>()
            };

            _mockRepo.Setup(repo => repo.Add(It.IsAny<Comment>())).ReturnsAsync((Comment comment) => comment);

            // Act
            await _dao.AddCommentAync(comment);

            // Assert
            _mockRepo.Verify(repo => repo.Add(It.IsAny<Comment>()), Times.Exactly(1));
        }

        [Fact]
        public async Task Update_Should_Return_Ok()
        {
            Comment comment = new() { Id = Guid.NewGuid().ToString(), Content = "Comment1" };

            _mockRepo.Setup(repo => repo.Update(It.IsAny<Comment>())).ReturnsAsync(UpdateStatus.Ok);

            UpdateStatus result = await _dao.UpdateCommentAsync(comment);

            Assert.Equal(UpdateStatus.Ok, result);
            _mockRepo.Verify(repo => repo.Update(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public async Task Update_Should_Handle_Concurrency_Conflict()
        {
            // Can't mock this for some reason, nothing returns the right value, do this for now!

            // Arrange
            CommentDAO dao1 = new();
            CommentDAO dao2 = new();

            AppUser user = new()
            {
                Id = Guid.NewGuid().ToString()
            };
            Post post = new()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Wow",
                AppUserId = user.Id,
                Content = "Cool content"
            };
            Comment comment = new()
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Wow!",
                Author = user,
                Post = post
            };

            await dao1.AddCommentAync(comment);

            Comment? testComment1 = await dao1.GetCommentByIdAsync(comment.Id);
            Comment? testComment2 = await dao2.GetCommentByIdAsync(comment.Id);

            testComment1!.Content = "Comment2";

            // Act
            if (await dao1.UpdateCommentAsync(testComment1) == UpdateStatus.Ok)
            {
                testComment2!.Content = "Comment3";
                // Assert
                Assert.True(await dao2.UpdateCommentAsync(testComment2) == UpdateStatus.Stale);
            }
            else
            {
                Assert.True(false);
            }

            _ = await dao1.DeleteCommentAsync(testComment1!.Id!);
            _ = _context.Posts.Remove(post);
            _ = _context.Users.Remove(user);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetById_Should_Return_Comment()
        {
            // Arrange
            Comment comment = new() { Id = Guid.NewGuid().ToString() };

            _mockRepo.Setup(repo => repo.GetOne(It.IsAny<Expression<Func<Comment, bool>>>())).ReturnsAsync(comment);

            // Act
            Comment? result = await _dao.GetCommentByIdAsync(comment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(comment, result);
            _mockRepo.Verify(repo => repo.GetOne(It.IsAny<Expression<Func<Comment, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Delete_1()
        {
            // Arrange
            Comment comment = new() { Id = Guid.NewGuid().ToString(), Content = "Comment1" };

            _mockRepo.Setup(repo => repo.Delete(comment.Id)).ReturnsAsync(1);

            // Act
            int result = await _dao.DeleteCommentAsync(comment.Id);

            // Assert
            Assert.Equal(1, result);
            _mockRepo.Verify(repo => repo.Delete(comment.Id), Times.Once);
        }
    }
}
