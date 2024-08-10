using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;

namespace DatabaseAccess;

public abstract class DbBase
{

    protected string DbPath { get; init; } = "C:\\Users\\ncoro\\source\\repos\\alman\\src\\db_access\\Database\\alman.db";
    protected virtual AlmanContext ConnectToDb()
    {
        if (string.IsNullOrWhiteSpace(DbPath))
        {
            return null;
        }
        var ctx = new AlmanContext(DbPath);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    public virtual void DeleteDb(string dbPath)
    {
        var ctx = new AlmanContext(DbPath);
        ctx.Database.EnsureDeleted();
    }

}


public class DbConnection : DbBase
{
    public DbConnection(string dbPath)
    {
        DbPath = dbPath;
    }

    public DbConnection() { }

    public TEntity GetItemById<TEntity>(int id)
        where TEntity : class, IIdentifier, new()
    {
        using var db = ConnectToDb();
        TEntity item;
        try
        {
            item = db.GetDeclaredDbSet<TEntity>().Where(it => it.Id == id).Single();
        }
        catch (Exception ex)
        {
            DebugUtilities.WriteExceptionToDebug(ex);
            item = new TEntity();
        }
        return item;
    }

    public IReadOnlyList<TEntity> GetItems<TEntity>(Func<TEntity, bool> selector)
        where TEntity : class
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.GetDeclaredDbSet<TEntity>());
    }

    public ReturnCode AddItems<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.GetDeclaredDbSet<TEntity>(), entities, db);
    }

    public ReturnCode UpdateItems<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(entities, db);
    }

    public ReturnCode DeleteItems<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class, IDeleteDependable
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(entities, db);
    }
}