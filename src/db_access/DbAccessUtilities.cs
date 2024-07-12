using System.Diagnostics;

using DbAccess.Models;
using System.Security.AccessControl;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess;


public static class DbAccessUtilities
{

    #region Debuguging

    public static void WriteExceptionToDebug(Exception exception)
    {
        Debug.WriteLine("----Another one----");
        Debug.Assert(exception != null);
        Debug.WriteLine(exception.ToString());
        Debug.WriteLine(exception.Message);
        Debug.WriteLine(exception.StackTrace);
        Debug.WriteLine("---------End--------");
    }
    
    #endregion


    public static List<TEntity> GetEntities<TEntities, TEntity>(Func<TEntity, bool> predicate, TEntities entities)
    where TEntity : class
    where TEntities : DbSet<TEntity>
    {
        List<TEntity> items;
        try
        {
            items = entities.Where(predicate).ToList();
        }
        catch (Exception ex)
        {
            WriteExceptionToDebug(ex);
            items = new List<TEntity>();
        }
        return items;
    }

}