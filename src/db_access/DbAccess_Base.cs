using Alman;
using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace DbAccess;

/// <summary>
/// Base class that represents basic functionality for any dbContext.
/// </summary>
public abstract class DbBase
{
    /// <summary>
    /// Path to the database file.
    /// </summary>
    protected string DbPath { get; set; } = "";

    /// <summary>
    /// Connect to the database, ensure it is created and has at least Admin user.
    /// </summary>
    /// <returns>Instance of the AlmanContext</returns>
    public virtual AlmanContext ConnectToDb()
    {
        if (string.IsNullOrWhiteSpace(DbPath))
        {
            DbPath = AlmanConfig.GetConfigString("Database:DataBaseFilePath");
        }
        var ctx = new AlmanContext(DbPath);
        ctx.Database.EnsureCreated();
        if (!ctx.Users.Any())
        {
            int id = int.Parse(AlmanConfig.GetConfigString("Database:AdminId"));
            int permissions = int.Parse(AlmanConfig.GetConfigString("Database:AdminPermissions"));
            ctx.Users.Add(new User { Name = AlmanConfig.GetConfigString("Database:AdminName"), Password = AlmanConfig.GetConfigString("Database:AdminPassword"), Id = id, Permissions = permissions });
            ctx.SaveChanges();
        }
        return ctx;
    }

    /// <summary>
    /// Delete .db file that stores the database.
    /// </summary>
    /// <param name="dbPath"> Path of the deatabes file to delete. </param>
    public virtual void DeleteDb(string dbPath)
    {
        var ctx = new AlmanContext(DbPath);
        ctx.Database.EnsureDeleted();
    }

}


/// <summary>
/// Class that represents connection to the database. Reperesents the API for the business layer and provides abstraction for Entity framework and chosen database system.
/// </summary>
public partial class DbConnection : DbBase
{
    /// <summary>
    /// ctor that accepts path to the database file.
    /// </summary>
    /// <param name="dbPath">Path to the database file that stores the dtabase.</param>
    public DbConnection(string dbPath)
    {
        DbPath = dbPath;
    }

    /// <summary>
    /// default ctor.
    /// </summary>
    public DbConnection() { }

    /// <summary>
    /// Get item of type <typeparamref name="TEntity"/> from database by its <paramref name="id"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that represents row of table in db</typeparam>
    /// <param name="id">Id of the row</param>
    /// <returns>Instance of the <typeparamref name="TEntity"/> or null</returns>
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

    /// <summary>
    /// Get the rows from the table of Entity <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that represents row of table in db.</typeparam>
    /// <param name="selector"> Condition for selectiong particular rows.</param>
    /// <returns>ReadOnly list of the items of type  <typeparamref name="TEntity"/>.</returns>
    public IReadOnlyList<TEntity> GetItems<TEntity>(Func<TEntity, bool> selector)
        where TEntity : class
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.GetDeclaredDbSet<TEntity>());
    }

    /// <summary>
    /// Add new rows to the table.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that represents row of table in db.</typeparam>
    /// <param name="entities"> New rows.</param>
    /// <returns>ReturnCode.OK if successful, ReturnCode.ERR otherwise. </returns>
    public ReturnCode AddItems<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.GetDeclaredDbSet<TEntity>(), entities, db);
    }

    /// <summary>
    /// Upadate rows from the table.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that represents row of table in db.</typeparam>
    /// <param name="entities">Rows to be updated.</param>
    /// <returns>ReturnCode.OK if successful, ReturnCode.ERR otherwise. </returns>
    public ReturnCode UpdateItems<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(entities, db);
    }

    /// <summary>
    /// Delete rows from the table.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that represents row of table in db.</typeparam>
    /// <param name="entities">List of items to be deleted. </param>
    /// <returns>ReturnCode.OK if successful, ReturnCode.ERR otherwise. </returns>
    public ReturnCode DeleteItems<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class, IDeleteDependable
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(entities, db);
    }
}


/// <summary>
/// Class that is needed purely for tests. The tets are running as pipeline in github and appconfig.json file is not pushed for security reasons. 
/// Therefore the class DbConnection cannot be used in tests.
/// </summary>
public partial class DbConnectionTest : DbConnection
{
    public DbConnectionTest(string dbPath)
    {
        DbPath = dbPath;
    }
    public override AlmanContext ConnectToDb()
    {
        var ctx = new AlmanContext(DbPath);
        ctx.Database.EnsureCreated();
        if (!ctx.Users.Any())
        {
            ctx.Users.Add(new User { Name = "Admin", Password = "1234", Id = 1, Permissions = 1 });
            ctx.SaveChanges();
        }
        return ctx;
    }
}