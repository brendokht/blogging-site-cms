using System;
using System.Reflection;

using BloggingSiteCMS.DAL.Domain;
using BloggingSiteCMS.DAL.Enums;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BloggingSiteCMS.DAL.DAO
{
    public class PostDAO
    {
        private readonly IRepository<Post> _repo;

        public PostDAO()
        {
            _repo = new CMSRepository<Post>();
        }

        /// <summary>
        /// Gets a Post object from the database by its ID
        /// </summary>
        /// <param name="postId">The ID of the Post object to grab</param>
        /// <returns>The Post object with the specified ID</returns>
        public async Task<Post> GetPostAsync(string postId)
        {
            Post? post;
            try
            {
                post = await _repo.GetOne(p => p.Id == postId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
            return post!;
        }

        /// <summary>
        /// Will Grab a List of Tags for a Post
        /// </summary>
        /// <param name="postId">ID of Post object to get Tag objects from</param>
        /// <returns>List of Tag objects</returns>
        public async Task<List<Tag>> GetTagsForPostAsync(string postId)
        {
            List<Tag> tags = new List<Tag>();
            try
            {
                Post? post = await _repo.GetOne(p => p.Id == postId);
                tags = post!.Tags.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
            return tags ?? new List<Tag>();
        }

        /// <summary>
        /// Add a Post to the database
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task AddPostAsync(Post post)
        {
            try
            {
                await _repo.Add(post);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates a singular Post object
        /// </summary>
        /// <param name="updatedPost">The Post object to be updated</param>
        /// <returns>An integer value representing the status of the update</returns>
        public async Task<UpdateStatus> UpdatePostAsync(Post updatedPost)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                status = await _repo.Update(updatedPost);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
            return status;
        }

        /// <summary>
        /// Deletes a singular Post object
        /// </summary>
        /// <param name="postId">The ID of the Post object to delete</param>
        /// <returns>An integer representing how many objects were deleted</returns>
        public async Task<int> DeletePostAsync(string postId)
        {
            int result = 0;
            try
            {
                result = await _repo.Delete(postId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
            return result;
        }
    }
}
