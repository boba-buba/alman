using System.Diagnostics;

using Alman.Models;
using System.Security.AccessControl;
//using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;


public static class DebugUtilities
{
    #region Debuguging

    public static void WriteExceptionToDebug(Exception exception)
    {
        Debug.Assert(exception != null);

        Debug.WriteLine("----Another one----");
        //Debug.Write("  Exception description: "); Debug.WriteLine(exception.ToString());
        Debug.WriteLine("  Exception message: "); Debug.WriteLine(exception.Message);
        Debug.WriteLine("  Exception stack trace: "); Debug.WriteLine(exception.StackTrace);
        Debug.WriteLine("---------End--------");
    }

    #endregion
}


public static class DbAccessUtilities
{
    public static List<TEntity> GetEntities<TEntity>(Func<TEntity, bool> predicate, DbSet<TEntity> entities)
    where TEntity : class
    {
        List<TEntity> items;
        try
        {
            items = entities.Where(predicate).ToList();
        }
        catch (Exception ex)
        {
            DebugUtilities.WriteExceptionToDebug(ex);
            items = new List<TEntity>();
        }
        return items;
    }

    public static ReturnCode AddEntities<TEntity, TDbContext>(DbSet<TEntity> entities, IEnumerable<TEntity> newEntities, TDbContext db)
        where TEntity : class
        where TDbContext : DbContext

    {
        entities.AddRange(newEntities);
        try
        {
            int changesCount = db.SaveChanges();
        }
        catch (Exception ex)
        {
            if (ex is DbUpdateConcurrencyException or DbUpdateException)
            {
                DebugUtilities.WriteExceptionToDebug(ex);
                return ReturnCode.SAVE_CTX_ERR;
            }
        }

        return ReturnCode.OK;
    }

    public static ReturnCode UpdateEntities<TEntity, TDbContext>(IEnumerable<TEntity> changedEntities, TDbContext db)
        where TEntity : class
        where TDbContext : DbContext
    {
        int changesCount;
        try
        {
            foreach (var entity in changedEntities)
            {
                db.Update(entity);
            }

            changesCount = db.SaveChanges();
        }
        catch (Exception ex)
        {
            DebugUtilities.WriteExceptionToDebug(ex);
            return ReturnCode.SAVE_CTX_ERR;
        }
        return ReturnCode.OK;
    }


    public static ReturnCode DeleteEntities<TEntity, TDbContext>(IEnumerable<TEntity> entitiesToDelete,  TDbContext db, Action<TDbContext, TEntity> DeleteFunction)
        where TEntity : class
        where TDbContext : DbContext
    {
        try
        {
            foreach (var entity in entitiesToDelete)
            {
                DeleteFunction(db, entity);

                db.Remove(entity);
            }

            db.SaveChanges();
        }
        catch (Exception ex)
        {
            DebugUtilities.WriteExceptionToDebug(ex);
            return ReturnCode.SAVE_CTX_ERR;
        }
        return ReturnCode.OK;
    }
}