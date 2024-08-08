using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using BloggingSiteCMS.DAL.Domain;
using BloggingSiteCMS.DAL.Enums;

using Microsoft.EntityFrameworkCore;

namespace BloggingSiteCMS.DAL
{
    public class CMSRepository<T> : IRepository<T> where T : CMSEntity
    {
        readonly private CMSContext _context;
        public CMSRepository()
        {
            _context = new CMSContext();
        }
        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<List<T>> GetSome(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }
        public async Task<T?> GetOne(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(match);
        }
        public async Task<T> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<UpdateStatus> Update(T updatedEntity)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                T? currentEntity = await GetOne(ent => ent.Id == updatedEntity.Id);
                _context.Entry(currentEntity!).OriginalValues["Version"] = updatedEntity.Version;
                _context.Entry(currentEntity!).CurrentValues.SetValues(updatedEntity);
                if (await _context.SaveChangesAsync() == 1) // should throw exception if stale;
                    operationStatus = UpdateStatus.Ok;
            }
            catch (DbUpdateConcurrencyException dbx)
            {
                operationStatus = UpdateStatus.Stale;
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod()!.Name + dbx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod()!.Name + ex.Message);
            }
            return operationStatus;
        }
        public async Task<int> Delete(string id)
        {
            T? currentEntity = await GetOne(ent => ent.Id == id);
            _context.Set<T>().Remove(currentEntity!);
            return _context.SaveChanges();
        }
    }
}