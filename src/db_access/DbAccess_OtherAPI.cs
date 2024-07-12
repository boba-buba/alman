using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Diagnostics;
using System.Security.AccessControl;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess;
using DbAccess.Models;


public class DbOther : DbBase, IAlmanOtherRead
{
    public IReadOnlyList<OtherActivity> GetOtherActivities()
    {
        using var db = ConnectToDb();
        Func<OtherActivity, bool> selector = (activity) => { return true; };
        return DbAccessUtilities.GetEntities(selector, db.OtherActivities);
    }

    public IReadOnlyList<YearMonthOther> GetYearMonthOthers()
    {
        using var db = ConnectToDb();
        Func<YearMonthOther, bool> selector = (other) => { return true; };
        return DbAccessUtilities.GetEntities(selector, db.YearMonthOthers);
    }
}