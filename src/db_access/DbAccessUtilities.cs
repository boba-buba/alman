using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Alman.SharedDefinitions;
namespace DbAccess;


public static class DebugUtilities
{
    #region Debuguging

    public static void WriteExceptionToDebug(Exception exception)
    {
        Debug.Assert(exception != null);

        Debug.WriteLine("----Another one----");
        Debug.WriteLine("  Exception message: "); Debug.WriteLine(exception.Message);
        Debug.WriteLine("  Exception stack trace: "); Debug.WriteLine(exception.StackTrace);
        Debug.WriteLine("---------End--------");
    }

    #endregion
}

/// <summary>
/// Provides mechanism for deleting dependable objects from database.
/// </summary>
public interface IDeleteDependable
{
    /// <summary>
    /// Delete all items from all tables that have reference to this object.
    /// </summary>
    /// <param name="dbContext"> Database to delete from. </param>
    void DeleteDependable(DbContext dbContext);
    
}

/// <summary>
/// Provide generic functions for managing dbSets in database.
/// </summary>
public static class DbAccessUtilities
{
    /// <summary>
    /// Get items from the table of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of the row of the table. </typeparam>
    /// <param name="predicate"> Condition on which particular items are chosen. </param>
    /// <param name="entities"> The table from which the items are chosen. </param>
    /// <returns>List of the read items. </returns>
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

    /// <summary>
    /// Add new items to the table of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of the row of the table. </typeparam>
    /// <typeparam name="TDbContext">Type of database to add items to. </typeparam>
    /// <param name="entities"> Table to add items to. </param>
    /// <param name="newEntities"> Items to add to the table. </param>
    /// <param name="db"> Database to add items to. </param>
    /// <returns> ReturnCode.OK if successful, ReturnCode.ERR otherwise. </returns>
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

    /// <summary>
    /// Update items from the table of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of the row of the table. </typeparam>
    /// <typeparam name="TDbContext">Type of database to update items in. </typeparam>
    /// <param name="changedEntities"> Changed items. </param>
    /// <param name="db"> Database to update items in.</param>
    /// <returns> ReturnCode.OK if successful, ReturnCode.ERR otherwise. </returns>
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

    /// <summary>
    /// Delete items of type <typeparamref name="TEntity"/> from database.
    /// </summary>
    /// <typeparam name="TEntity">Type of the row of the table. </typeparam>
    /// <typeparam name="TDbContext">Type of database to delete items from. </typeparam>
    /// <param name="entitiesToDelete"></param>
    /// <param name="db"></param>
    /// <returns> ReturnCode.OK if successful, ReturnCode.ERR otherwise. </returns>
    public static ReturnCode DeleteEntities<TEntity, TDbContext>(IEnumerable<TEntity> entitiesToDelete,  TDbContext db)
        where TEntity : class, IDeleteDependable
        where TDbContext : DbContext
    {
        try
        {
            foreach (var entity in entitiesToDelete)
            {
                entity.DeleteDependable(db);

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