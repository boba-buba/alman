using DbAccess.Models;

namespace DatabaseAccess;

public abstract class DbBase
{
    public DbBase() { }

    protected virtual AlmanContext ConnectToDb()
    {
        return new AlmanContext("full path");
    }

}