using System;
using System.Reflection;

using BloggingSiteCMS.DAL.Domain;
using BloggingSiteCMS.DAL.Enums;

using Microsoft.EntityFrameworkCore;

namespace BloggingSiteCMS.DAL.DAO
{
    public class CommentDAO
    {
        private readonly IRepository<Comment> _repo;

        public CommentDAO()
        {
            _repo = new CMSRepository<Comment>();
        }

        public CommentDAO(IRepository<Comment> repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Gets a Comment object from the database by its ID
        /// </summary>
        /// <param name="commentId">The ID of the Comment object to grab</param>
        /// <returns>The Comment object if found, else returns null</returns>
        public async Task<Comment?> GetCommentByIdAsync(string commentId)
        {
            Comment? comment = null;
            try
            {
                comment = await _repo.GetOne(c => c.Id == commentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
            }
            return comment;
        }

        /// <summary>
        /// Adds a Comment to the database
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task AddCommentAync(Comment comment)
        {
            try
            {
                await _repo.Add(comment);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates a singular Comment object
        /// </summary>
        /// <param name="updatedComment">The Comment object to be updated</param>
        /// <returns>An integer value representing the status of the update</returns>
        public async Task<UpdateStatus> UpdateCommentAsync(Comment updatedComment)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                status = await _repo.Update(updatedComment);
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
        /// Deletes a singular Comment object
        /// </summary>
        /// <param name="commentId">The ID of the Comment object to delete</param>
        /// <returns>An integer representing how many objects were deleted</returns>
        public async Task<int> DeleteCommentAsync(string commentId)
        {
            int result;
            try
            {
                result = await _repo.Delete(commentId);
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
