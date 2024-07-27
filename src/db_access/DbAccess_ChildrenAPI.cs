using DbAccess.Models;
using Microsoft.EntityFrameworkCore;
using Alman.SharedDefinitions;

namespace DatabaseAccess;

public class DbChildren : DbBase, IAlmanChildrenRead, IAlmanChildrenWrite
{
    public DbChildren(string dbPath)
    {
        DbPath = dbPath;
    }
    
    public DbChildren() { }

    #region Children Reading

    //private Func<Child, bool> defaultSelector = ch => true;
    public IReadOnlyList<Child> GetChildren(Func<Child, bool> selector)
    {
        //Func<Child, bool> selector = (child) => { return true; };

        using var db = ConnectToDb();

        return DbAccessUtilities.GetEntities(selector, db.Children);
    }

    /* Children activities list **/
    public IReadOnlyList<DbAccess.Models.Activity> GetActivities(Func<Activity, bool> selector)
    {
        using var db = ConnectToDb();

        return DbAccessUtilities.GetEntities(selector, db.Activities);
    }

    public IReadOnlyList<Precontract> GetPrecontracts(Func<Precontract, bool> selector)
    {
        using var db = ConnectToDb();

        return DbAccessUtilities.GetEntities(selector, db.Precontracts);
    }

    /* Year Month Table for the month of the year
     * 
     */
    public IReadOnlyList<YearMonthActivity> GetYearMonthActivities(Func<YearMonthActivity, bool> selector)
    {
        using var db = ConnectToDb();

        return DbAccessUtilities.GetEntities(selector, db.YearMonthActivities);
    }

    public IReadOnlyList<YearSub> GetYearSubs(Func<YearSub, bool> selector)
    {
        using var db = ConnectToDb();

        return DbAccessUtilities.GetEntities(selector, db.YearSubs);
    }

    public IReadOnlyList<ContractFee> GetContractFees(Func<ContractFee, bool> selector)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.GetEntities(selector, db.ContractFees);
    }

    public IReadOnlyList<Child> GetChildrenByName(string firstName, string lastName)
    {
        using var db = ConnectToDb();
        Func<Child, bool> selectror = (child) => { return child.ChildName == firstName && child.ChildLastName == lastName; };
        return DbAccessUtilities.GetEntities(selectror, db.Children);
    }

    /*
     * Must be only one child with such ChildId. Primary key **/
    public Child GetChildById(int ChildId)
    {
        using var db = ConnectToDb();
        Child child;
        try
        {
            child = db.Children.Where(ch => ch.ChildId == ChildId).Single();
        }
        catch (Exception ex)
        {
            DebugUtilities.WriteExceptionToDebug(ex);
            child = new Child();
        }
        return child;
    }

    public Precontract GetPrecontractById(int ChildId)
    {
        using var db = ConnectToDb();
        Precontract precontract;
        try
        {
            //kvuli single by volani te getEntities az tak moc nedavalo smysl, stale je potreba try block
            precontract = db.Precontracts.Single(pr => pr.PchildId == ChildId);
        }
        catch (Exception ex)
        {
            DebugUtilities.WriteExceptionToDebug(ex);
            precontract = new Precontract();
        }
        return precontract;
    }

    /* Get all child's activities for month. */
    public IReadOnlyList<YearMonthActivity> GetYearMonthActivitiesById(int year, int month, int ChildId)
    {
        using var db = ConnectToDb();
        Func<YearMonthActivity, bool> selector = (activity) => { return activity.Year == year && activity.Month == month && activity.YmchildId == ChildId; };
        return DbAccessUtilities.GetEntities(selector, db.YearMonthActivities);
    }

    public YearSub GetChildYearSubById(int year, int ChildId)
    {
        using var db = ConnectToDb();
        YearSub ySub;

        try
        {
            ySub = db.YearSubs.Single(
                ySubscription =>
                ySubscription.Yyear == year &&
                ySubscription.YchildId == ChildId
             );
        }
        catch (Exception ex)
        {
            DebugUtilities.WriteExceptionToDebug(ex);
            ySub = new YearSub();
        }

        return ySub;
    }

    public ContractFee GetContractFeeById(int year, int month, int ChildId)
    {
        using var db = ConnectToDb();
        ContractFee contractFee;
        try
        {
            contractFee = db.ContractFees.Single(
                contFee =>
                contFee.Cfyear == year &&
                contFee.Cfmonth == month &&
                contFee.CfchildId == ChildId
                );
        }
        catch (Exception ex)
        {
            DebugUtilities.WriteExceptionToDebug(ex);
            contractFee = new ContractFee();
        }

        return contractFee;
    }
    #endregion

    //Tables that have foreign keys: rows can be added through children, but not updated  
    #region Children writing
    public ReturnCode AddChildren(IEnumerable<Child> children)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.Children, children, db);
    }

    public ReturnCode UpdateChildren(IEnumerable<Child> children)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(children, db);
    }

    private void DeleteDependableOnChildRows(AlmanContext db, Child child)
    {
        db.RemoveRange(db.Precontracts.Where(pr => pr.PchildId == child.ChildId).ToList());
        db.RemoveRange(db.YearMonthActivities.Where(ymAc => ymAc.YmchildId == child.ChildId).ToList());
        db.RemoveRange(db.YearSubs.Where(ys => ys.YchildId == child.ChildId).ToList());
        db.RemoveRange(db.ContractFees.Where(cf => cf.CfchildId == child.ChildId).ToList());
    }
    public ReturnCode DeleteChildren(IEnumerable<Child> children)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(children, db, DeleteDependableOnChildRows);
    }



    public ReturnCode AddActvities(IEnumerable<Activity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.Activities, activities, db);
    }

    public ReturnCode UpdateActvities(IEnumerable<Activity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(activities, db);
    }

    private void DeleteDependableOnActivityRows(AlmanContext db, Activity activity)
    {
        db.RemoveRange(db.YearMonthActivities.Where(ymAc => ymAc.YmactivityId == activity.ActivityId).ToList());
    }
    public ReturnCode DeleteActvities(IEnumerable<Activity> activities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(activities, db, DeleteDependableOnActivityRows);
    }



    public ReturnCode AddPrecontracts(IEnumerable<Precontract> precontracts)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.Precontracts, precontracts, db);
    }

    public ReturnCode UpdatePrecontracts(IEnumerable<Precontract> precontracts)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(precontracts, db);
    }


    private void DeleteDependableOnPrecontractRows(AlmanContext db, Precontract precontract) { }
    public ReturnCode DeletePrecontracts(IEnumerable<Precontract> precontracts)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(precontracts, db, DeleteDependableOnPrecontractRows);
    }


    //can be done through child
    public ReturnCode AddYearMonthActivities(IEnumerable<YearMonthActivity> yearMonthActivities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.YearMonthActivities, yearMonthActivities, db);
    }

    public ReturnCode UpdateYearMonthActivities(IEnumerable<YearMonthActivity> yearMonthActivities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(yearMonthActivities, db);
    }

    private void DeleteDependableOnYearMonthActivityRows(AlmanContext db, YearMonthActivity yearMonthActivity) { }
    public ReturnCode DeleteYearMonthActivities(IEnumerable<YearMonthActivity> yearMonthActivities)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(yearMonthActivities, db, DeleteDependableOnYearMonthActivityRows);
    }


    //can be done through child
    public ReturnCode AddYearSubs(IEnumerable<YearSub> yearSubs)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.YearSubs, yearSubs, db);
    }

    public ReturnCode UpdateYearSubs(IEnumerable<YearSub> yearSubs)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(yearSubs, db);
    }

    private void DeleteDependableOnYearSubRows(AlmanContext db, YearSub ySub) { }
    public ReturnCode DeleteYearSubs(IEnumerable<YearSub> yearSubs)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(yearSubs, db, DeleteDependableOnYearSubRows);
    }

    //can be done through child
    public ReturnCode AddContractFees(IEnumerable<ContractFee> contractFees)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.AddEntities(db.ContractFees, contractFees, db);
    }

    public ReturnCode UpdateContractFees(IEnumerable<ContractFee> contractFees)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.UpdateEntities(contractFees, db);
    }

    private void DeleteDependableOnContractFeeRows(AlmanContext db, ContractFee contractFee) { }
    public ReturnCode DeleteContractFees(IEnumerable<ContractFee> contractFees)
    {
        using var db = ConnectToDb();
        return DbAccessUtilities.DeleteEntities(contractFees, db, DeleteDependableOnContractFeeRows);
    }
    #endregion
}

