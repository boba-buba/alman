using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Diagnostics;

using DbAccess.Models;
using System.Security.AccessControl;
//using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DatabaseAccess;


public class DbStaff : DbBase, IAlmanStaffRead, IAlmanStaffWrite
{
    #region Staff members reading
    public IReadOnlyList<StaffMember> GetStaffMembers(Func<StaffMember, bool> selector)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.StaffMembers);
    }


    public IReadOnlyList<Position> GetPositions(Func<Position, bool> selector)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.Positions);
    }


    public IReadOnlyList<Prepayment> GetPrepaymentsForYearMonth(Func<Prepayment, bool> selector)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.Prepayments);
    }


    public IReadOnlyList<FinalPayment> GetFinalPaymentsForYearMonth(Func<FinalPayment, bool> selector)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.FinalPayments);
    }

 
    public IReadOnlyList<StaffActivity> GetStaffActivities(Func<StaffActivity, bool> selector)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.StaffActivities);
    }


    public IEnumerable<YearMonthStaffActivity> GetYearMonthStaffActivitiesById(Func<YearMonthStaffActivity, bool> selector)
    {
        using var db = ConnectToDb();
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

    #region Staff members writing
    public ReturnCode AddNewStaffMembers(IEnumerable<StaffMember> members)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.StaffMembers, members, db);
    }
    public ReturnCode UpdateStaffMembers(IEnumerable<StaffMember> members)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(members, db);
    }
    private void DeleteDependableOnStaffMemberRows(AlmanContext db, StaffMember member)
    {
        db.RemoveRange(db.FinalPayments.Where(payment => payment.StaffMemberId == member.StaffMemberId).ToList());
        db.RemoveRange(db.Prepayments.Where(payment => payment.StaffMemberId == member.StaffMemberId).ToList());
        db.RemoveRange(db.YearMonthStaffActivities.Where(activity => activity.StaffMemberId == member.StaffMemberId).ToList());
    }
    public ReturnCode DeleteStaffMembers(IEnumerable<StaffMember> members)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(members, db, DeleteDependableOnStaffMemberRows);
    }

    public ReturnCode AddPositions(IEnumerable<Position> positions)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.Positions, positions, db);
    }
    public ReturnCode UpdatePositions(IEnumerable<Position> positions)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(positions, db);
    }
    private void DeleteDependableOnPositionRows(AlmanContext db, Position position) //???
    {
        List<StaffMember> staffMembers = db.StaffMembers.Where(staffMember => staffMember.PositionId == position.PositionId).ToList();
        DbAccessUtilities.DeleteEntities(staffMembers, db, DeleteDependableOnStaffMemberRows);
    }
    public ReturnCode DeletePositions(IEnumerable<Position> positions)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(positions, db, DeleteDependableOnPositionRows);
    }

    public ReturnCode AddPrepayments(IEnumerable<Prepayment> prepayments)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.Prepayments, prepayments, db);
    }
    public ReturnCode UpdatePrepayments(IEnumerable<Prepayment> prepayments)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(prepayments, db);
    }
    private void DeleteDependableOnPrepaymentRows(AlmanContext db, Prepayment prepayment) { }
    public ReturnCode DeletePrepayments(IEnumerable<Prepayment> prepayments)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(prepayments, db, DeleteDependableOnPrepaymentRows);
    }

    public ReturnCode AddFinalPayments(IEnumerable<FinalPayment> finalPayments)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.FinalPayments, finalPayments, db);
    }
    public ReturnCode UpdateFinalPayments(IEnumerable<FinalPayment> finalPayments)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(finalPayments, db);
    }
    private void DeleteDependableOnFinalPaymentRows(AlmanContext db, FinalPayment finalPayment) { }
    public ReturnCode DeleteFinalPayments(IEnumerable<FinalPayment> finalPayments)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(finalPayments, db, DeleteDependableOnFinalPaymentRows);
    }

    public ReturnCode AddStaffActivities(IEnumerable<StaffActivity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.StaffActivities, activities, db);
    }
    public ReturnCode UpdateStaffActivity(IEnumerable<StaffActivity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(activities, db);
    }
    private void DeleteDependableOnStaffActivityRows(AlmanContext db, StaffActivity staffActivity)
    {
        db.RemoveRange(db.YearMonthStaffActivities.Where(activity => activity.StaffActivityId == staffActivity.StaffActivityId).ToList());
    }
    public ReturnCode DeleteStaffActivities(IEnumerable<StaffActivity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(activities, db, DeleteDependableOnStaffActivityRows);
    }

    public ReturnCode AddYearMonthStaffActivities(IEnumerable<YearMonthStaffActivity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.YearMonthStaffActivities, activities, db);
    }
    public ReturnCode UpdateYearMonthStaffActivities(IEnumerable<YearMonthStaffActivity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(activities, db);
    }
    private void DeleteDependableOnYearMonthStaffActivityRows(AlmanContext db, YearMonthStaffActivity activity) { }
    public ReturnCode DeleteYearMonthStaffActivities(IEnumerable<YearMonthStaffActivity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(activities, db, DeleteDependableOnYearMonthStaffActivityRows);
    }

    #endregion
}