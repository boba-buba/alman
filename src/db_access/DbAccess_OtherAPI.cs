using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Diagnostics;
using System.Security.AccessControl;
//using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess;
using DbAccess.Models;
using Alman.SharedDefinitions;

public class DbOther : DbBase, IAlmanOtherRead, IAlmanOtherWrite
{
    #region Other reading
    public IReadOnlyList<OtherActivity> GetOtherActivities(Func<OtherActivity, bool> selector)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.OtherActivities);
    }

    public IReadOnlyList<YearMonthOther> GetYearMonthOthers(Func<YearMonthOther, bool> selector)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.YearMonthOthers);
    }
    #endregion

    #region Other writing

    public ReturnCode AddOtherActivities(IEnumerable<OtherActivity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.OtherActivities, activities, db);
    }
    public ReturnCode UpdateOtherActivities(IEnumerable<OtherActivity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(activities, db);
    }
    private void DeleteDependableOnOtherActivitiesRows(AlmanContext db, OtherActivity otherActivity)
    {
        db.RemoveRange(db.YearMonthOthers.Where(activity =>activity.OtherActivityId == otherActivity.OtherId).ToList());
    }
    public ReturnCode DeleteOtherActivities(IEnumerable<OtherActivity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(activities, db, DeleteDependableOnOtherActivitiesRows);
    }

    public ReturnCode AddYearMonthOthers(IEnumerable<YearMonthOther> others)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.YearMonthOthers, others, db);
    }
    public ReturnCode UpdateYearMonthOthers(IEnumerable<YearMonthOther> others)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(others, db);
    }
    private void DeleteDependableOnYearMonthOtherRows(AlmanContext db, YearMonthOther other) { }
    public ReturnCode DeleteYearMonthOthers(IEnumerable<YearMonthOther> others)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(others, db, DeleteDependableOnYearMonthOtherRows);
    }
    #endregion

}