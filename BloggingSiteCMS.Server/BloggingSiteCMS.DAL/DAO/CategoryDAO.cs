using System;
using System.Reflection;

using BloggingSiteCMS.DAL.Domain;
using BloggingSiteCMS.DAL.Enums;

using Microsoft.EntityFrameworkCore;

namespace BloggingSiteCMS.DAL.DAO
{
    public class CategoryDAO
    {
        private readonly IRepository<Category> _repo;

        public CategoryDAO()
        {
            _repo = new CMSRepository<Category>();
        }

        public CategoryDAO(IRepository<Category> repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Gets a Category object from the database by its ID
        /// </summary>
        /// <param name="categoryId">The ID of the Category object to grab</param>
        /// <returns>The Category object if found, else returns null</returns>
        public async Task<Category?> GetCategoryByIdAsync(string categoryId)
        {
            Category? category = null;
            try
            {
                category = await _repo.GetOne(c => c.Id == categoryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
            }
            return category;
        }

        /// <summary>
        /// Gets a Category object from the database by its Name
        /// </summary>
        /// <param name="categoryName">The Name of the Category object to grab</param>
        /// <returns>The Category object if found, else returns null</returns>
        public async Task<Category?> GetCategoryByNameAsync(string categoryName)
        {
            Category? category = null;
            try
            {
                category = await _repo.GetOne(c => c.Name == categoryName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
            }
            return category;
        }

        /// <summary>
        /// Add a List of Categories to the database
        /// </summary>
        /// <param name="categories">List of Category objects</param>
        public async Task AddCategoriesAsync(List<Category> categories)
        {
            try
            {
                foreach (var category in categories)
                {
                    // TODO: Make a database index for Category.Name
                    if (await GetCategoryByNameAsync(category.Name!) == null)
                        await _repo.Add(category);
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
        /// Updates a singular Category object
        /// </summary>
        /// <param name="updatedCategory">The Category object to be updated</param>
        /// <returns>An integer value representing the status of the update</returns>
        public async Task<UpdateStatus> UpdateCategoryAsync(Category updatedCategory)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                status = await _repo.Update(updatedCategory);
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
        /// Deletes a singular Category object
        /// </summary>
        /// <param name="categoryId">The ID of the Category object to delete</param>
        /// <returns>An integer representing how many objects were deleted</returns>
        public async Task<int> DeleteCategoryAsync(string categoryId)
        {
            int result;
            try
            {
                result = await _repo.Delete(categoryId);
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
