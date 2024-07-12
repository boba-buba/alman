using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Diagnostics;

using DbAccess.Models;
using System.Security.AccessControl;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess;

public abstract class DbBase
{
    public DbBase() { }

    protected virtual AlmanContext ConnectToDb()
    {
        return new AlmanContext("full path");
    }

}