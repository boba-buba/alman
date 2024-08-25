using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace DbAccess;

public abstract class DbBase
{

    protected string DbPath { get; init; } = "C:\\Users\\ncoro\\source\\repos\\alman\\src\\db_access\\Database\\alman.db";
    public virtual AlmanContext ConnectToDb()
    {
        if (string.IsNullOrWhiteSpace(DbPath))
        {
            throw new ArgumentNullException("Database path");
        }
        var ctx = new AlmanContext(DbPath);
        ctx.Database.EnsureCreated();
        if (!ctx.Users.Any())
        {
            ctx.Users.Add(new User { Name = "Admin", Password = "1234", Id = 1, Permissions = 1 });
            ctx.SaveChanges();
        }
        return ctx;
    }

    public virtual void DeleteDb(string dbPath)
    {
        var ctx = new AlmanContext(DbPath);
        ctx.Database.EnsureDeleted();
    }

}


public partial class DbConnection : DbBase
{
    public DbConnection(string dbPath)
    {
        DbPath = dbPath;
    }

    public DbConnection() { }

    public TEntity? GetItemById<TEntity>(int id)
        where TEntity : class, IIdentifier, new()
    {
        using var db = ConnectToDb();
        TEntity? item;
        try
        {
            item = db.GetDeclaredDbSet<TEntity>().Where(it => it.Id == id).SingleOrDefault();
        }
        catch (Exception ex)
        {
            return null;
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