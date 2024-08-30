using System.Reflection;

using BloggingSiteCMS.DAL.Domain;
using BloggingSiteCMS.DAL.Enums;

using Microsoft.EntityFrameworkCore;

namespace BloggingSiteCMS.DAL.DAO
{
    public class PostDAO
    {
        private readonly IRepository<Post> _repo;

        public PostDAO()
        {
            _repo = new CMSRepository<Post>();
        }

        public PostDAO(IRepository<Post> repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Gets a Post object from the database by its ID
        /// </summary>
        /// <param name="postId">The ID of the Post object to grab</param>
        /// <returns>The Post object if found, else returns null</returns>
        public async Task<Post?> GetPostByIdAsync(string postId)
        {
            Post? post = null;
            try
            {
                post = await _repo.GetOne(p => p.Id == postId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
            }
            return post;
        }

        /// <summary>
        /// Will Grab a List of Tags for a Post
        /// </summary>
        /// <param name="postId">ID of Post object to get Tag objects from</param>
        /// <returns>List of Tag objects</returns>
        public async Task<List<Tag>?> GetTagsForPostAsync(string postId)
        {
            List<Tag> tags = [];
            try
            {
                Post? post = await _repo.GetOne(p => p.Id == postId);
                // TODO: Ensure using '!' operator here is good practice
                tags = [.. post!.Tags!];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
            return tags;
        }

        /// <summary>
        /// Will Grab a List of Comments for a Post
        /// </summary>
        /// <param name="postId">ID of Post object to get Tag objects from</param>
        /// <returns>List of Comment objects</returns>
        public async Task<List<Comment>?> GetCommentsForPostAsync(string postId)
        {
            List<Comment> comments = [];
            try
            {
                Post? post = await _repo.GetOne(p => p.Id == postId);
                // TODO: Ensure using '!' operator here is good practice
                comments = [.. post!.Comments!];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
            return comments;
        }

        /// <summary>
        /// Adds a Post to the database
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task AddPostAsync(Post post)
        {
            try
            {
                if (post.Tags != null && post.Tags.Any())
                {
                    IRepository<Tag> _tagRepo = new CMSRepository<Tag>();
                    List<Tag> tagsToAdd = new List<Tag>();

                    foreach (Tag tag in post.Tags)
                    {
                        var existingTag = await _tagRepo.GetOne(t => t.Name == tag.Name);

                        if (existingTag != null)
                        {
                            tagsToAdd.Add(existingTag);
                        }
                        else
                        {
                            tagsToAdd.Add(tag);
                        }
                    }

                    post.Tags = new List<Tag>(tagsToAdd);
                }

                if (post.Categories != null && post.Categories.Any())
                {
                    IRepository<Category> _categoryRepo = new CMSRepository<Category>();
                    List<Category> categoriesToAdd = new List<Category>();

                    foreach (Category category in post.Categories)
                    {
                        var existingTag = await _categoryRepo.GetOne(c => c.Name == category.Name);

                        if (existingTag != null)
                        {
                            categoriesToAdd.Add(existingTag);
                        }
                        else
                        {
                            categoriesToAdd.Add(category);
                        }
                    }

                    post.Categories = new List<Category>(categoriesToAdd);
                }

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
            catch (DbUpdateConcurrencyException dbx)
            {
                Console.WriteLine($"Concurrency exception in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {dbx.Message}");
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
            int result;
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
