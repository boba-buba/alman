using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Diagnostics;

using DbAccess.Models;
using System.Security.AccessControl;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess;


public class DbStaff : DbBase, IAlmanStaffRead
{
    #region Staff members reading
    public IReadOnlyList<StaffMember> GetStaffMembers()
    {
        using var db = ConnectToDb();
        Func<StaffMember, bool> selector = (staffMember) => { return true; };
        return DbAccessUtilities.GetEntities(selector, db.StaffMembers);
    }


    public IReadOnlyList<Position> GetPositions()
    {
        using var db = ConnectToDb();
        Func<Position, bool> selector = (position) => { return true; };
        return DbAccessUtilities.GetEntities(selector, db.Positions);
    }


    public IReadOnlyList<Prepayment> GetPrepaymentsForYearMonth(int month, int year)
    {
        using var db = ConnectToDb();
        Func<Prepayment, bool> selector = (prepayment) => { return prepayment.Year == year && prepayment.Month == month; };
        return DbAccessUtilities.GetEntities(selector, db.Prepayments);
    }


    public IReadOnlyList<FinalPayment> GetFinalPaymentsForYearMonth(int month, int year)
    {
        using var db = ConnectToDb();
        Func<FinalPayment, bool> selector = (finPayment) => { return finPayment.Year == year && finPayment.Month == month; };
        return DbAccessUtilities.GetEntities(selector, db.FinalPayments);
    }

 
    public IReadOnlyList<StaffActivity> GetStaffActivities()
    {
        using var db = ConnectToDb();
        Func<StaffActivity, bool> selector = (staffActivity) => { return true; };
        return DbAccessUtilities.GetEntities(selector, db.StaffActivities);
    }


    public IEnumerable<YearMonthStaffActivity> GetYearMonthStaffActivitiesById(int month, int year)
    {
        using var db = ConnectToDb();
        Func<YearMonthStaffActivity, bool> selector = (ymsActivity) => { return ymsActivity.Month == month && ymsActivity.Year == year; };
        return DbAccessUtilities.GetEntities(selector, db.YearMonthStaffActivities);
    }


    public StaffMember GetStaffMemberById(int id)
    {
        using var db = ConnectToDb();
        StaffMember staffMember;
        try
        {
            staffMember = db.StaffMembers.Single(member => member.StaffMemberId == id);
        }
        catch
        (Exception ex)
        {
            DebugUtilities.WriteExceptionToDebug(ex);
            staffMember = new StaffMember();
        }
        return staffMember;
    }


    public IReadOnlyList<StaffMember> GetStaffMembersByName(string firstName, string lastName)
    {
        using var db = ConnectToDb();
        Func<StaffMember, bool> selector = (staffMmember) => { return staffMmember.FirstName == firstName && staffMmember.LastName == lastName; };
        return DbAccessUtilities.GetEntities(selector, db.StaffMembers);
    }
    #endregion
}