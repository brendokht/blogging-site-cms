﻿using System;
using System.Reflection;

using BloggingSiteCMS.DAL.Domain;
using BloggingSiteCMS.DAL.Enums;

using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Gets a Tag object from the database by its Id
        /// </summary>
        /// <param name="tagName">ID of the tag</param>
        /// <returns>The Tag object if found, else returns null</returns>
        public async Task<Tag?> GetTagByIdAsync(string tagId)
        {
            Tag? tag = null;
            try
            {
                tag = await _repo.GetOne(t => t.Id == tagId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
            return tag;
        }

        /// <summary>
        /// Gets a Tag object from the database by its Name
        /// </summary>
        /// <param name="tagName">Name of the tag</param>
        /// <returns>The Tag object if found, else returns null</returns>
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
                    // TODO: Make a database index for Tag.Name
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
            int result;
            try
            {
                result = await _repo.Delete(tagId);
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
