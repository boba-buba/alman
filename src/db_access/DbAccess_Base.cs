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