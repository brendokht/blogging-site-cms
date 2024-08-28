using System;
using System.Reflection;

using BloggingSiteCMS.DAL.Domain;

using Microsoft.EntityFrameworkCore;

using UpdateStatus = BloggingSiteCMS.DAL.Enums.UpdateStatus;

namespace BloggingSiteCMS.DAL.DAO
{
    public class TagDAO
    {
        private readonly IRepository<Tag> _repo;

        public TagDAO()
        {
            _repo = new CMSRepository<Tag>();
        }

        public TagDAO(IRepository<Tag> repo)
        {
            _repo = repo;
        }

        public async Task<Tag?> GetTagByNameAsync(string tagName)
        {
            Tag? tag = null;
            try
            {
                tag = await _repo.GetOne(t => t.Name == tagName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
            return tag;
        }

        /// <summary>
        /// Add a List of Tags to the database
        /// </summary>
        /// <param name="tags">List of Tag objects</param>
        public async Task AddTagsAsync(List<Tag> tags)
        {
            try
            {
                foreach (var tag in tags)
                {
                    if (await GetTagByNameAsync(tag.Name!) == null)
                        await _repo.Add(tag);
                    else
                        continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates a singular Tag object
        /// </summary>
        /// <param name="updatedTag">The Tag object to be updated</param>
        /// <returns>An integer value representing the status of the update</returns>
        public async Task<UpdateStatus> UpdateTagAsync(Tag updatedTag)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                status = await _repo.Update(updatedTag);
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
        /// Deletes a singular Tag object
        /// </summary>
        /// <param name="tagId">The ID of the Tag object to delete</param>
        /// <returns>An integer representing how many objects were deleted</returns>
        public async Task<int> DeleteTagAsync(string tagId)
        {
            int result = 0;
            try
            {
                result = await _repo.Delete(tagId);
            }
            catch (Exception ex)
            {
                throw;
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
            }
            return result;
        }
    }
}
