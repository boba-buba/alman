using Microsoft.Data.Sqlite;
using System.Data.Common;
//using System.Data.Entity;
using System.Diagnostics;

using DbAccess.Models;
using System.Security.AccessControl;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess;

public partial class DbAccsess_Draft
{

    private AlmanContext ConnectToDb()
    {
        return new AlmanContext("full path");
    }

    public YearSub GetChildYearSubById(AlmanContext ctx, int year, int ChildId)
    {
        //using var ctx = ConnectToDb();
        return ctx.YearSubs.Where(ys => ys.Yyear == year && ys.YchildId == ChildId).Single();
    }


    public AlmanDefinitions.ReturnCode UpdateYearSubs(IEnumerable<YearSub> yearSubs)
    {
        /*        using var ctx = ConnectToDb();

                foreach (var yearSub in yearSubs)
                {
                    //var dbYearSub = ctx.YearSubs.Where(ys => ys.YchildId == yearSub.YchildId && ys.Yyear == yearSub.Yyear).Single();
                    ctx.YearSubs.Update(yearSub);
                }
                ctx.SaveChanges();
                return AlmanDefinitions.ReturnCode.OK;*/

        using var ctx = ConnectToDb();
        var children = ctx.Children;

        //var ch = db.GetChildById(6);
        //db.DeleteChildren( new[] { ch });
        //var li = new List<YearSub>();
        foreach (var child in children)
        {
            //child.ChildContract = (int)AlmanDefinitions.ContractType.StaffChild;
            //child.YearSubs.Add(new DbAccess.Models.YearSub { Yyear = 2024, Yjune = 2000, YjunePayment = (int)AlmanDefinitions.WayOfPaying.Transfer });
            //var yearSub = GetChildYearSubById(ctx, 2024, child.ChildId);
            //child.YearSubs.Single().Yyear = 2026;
            //child.YearSubs.Single().YoctoberPayment = (int)AlmanDefinitions.WayOfPaying.Cash;
            child.YearSubs.Single().Yoctober = 2000;
            //ctx.YearSubs.Update(yearSub);
            //li.Add(yearSub);s
        }
        ctx.SaveChanges();
        return AlmanDefinitions.ReturnCode.OK;
    }

    public async Task UpdateYearSubsAsync(IEnumerable<YearSub> yearSubs)
    {
        using var ctx = ConnectToDb();

        foreach (var yearSub in yearSubs)
        {
            //var dbYearSub = ctx.YearSubs.Where(ys => ys.YchildId == yearSub.YchildId && ys.Yyear == yearSub.Yyear).Single();
            ctx.YearSubs.Update(yearSub);
        }
        
        await ctx.SaveChangesAsync();
    }

    public AlmanDefinitions.ReturnCode AddYearSubs(IEnumerable<YearSub> yearSubs)
    {
        using var ctx = ConnectToDb();
        foreach (var yearSub in yearSubs)
        {
            ctx.YearSubs.Add(yearSub);
        }
        ctx.SaveChanges();
        return AlmanDefinitions.ReturnCode.OK;
    }

}



public class DbChildren : DbBase, IAlmanChildrenRead
{
    
    #region Children Reading
    public IReadOnlyList<Child> GetChildren()
    {
        Func<Child, bool> selector = (child) => { return true; };

        using var db = ConnectToDb(); 
        
        return DbAccessUtilities.GetEntities(selector, db.Children);
    }

    /* Children activities list **/
    public IReadOnlyList<DbAccess.Models.Activity> GetActivities()
    {
        using var db = ConnectToDb();

        Func<DbAccess.Models.Activity, bool> selector = (activity) => { return true; };

        return DbAccessUtilities.GetEntities(selector, db.Activities);
    }

    public IReadOnlyList<Precontract> GetPrecontracts()
    {
        using var db = ConnectToDb();

        Func<Precontract, bool> selector = (precontract) => { return true; };

        return DbAccessUtilities.GetEntities(selector, db.Precontracts);
    }

    /* Year Month Table for the month of the year
     * 
     */
    public IReadOnlyList<YearMonthActivity> GetYearMonthActivities(int month, int year)
    {
        using var db = ConnectToDb();

        Func<YearMonthActivity, bool> selector = (activity) => { return activity.Month == month && activity.Year == year; };

        return DbAccessUtilities.GetEntities(selector, db.YearMonthActivities);
    }

    public IReadOnlyList<YearSub> GetYearSubs(int year)
    {
        using var db = ConnectToDb();
        Func<YearSub, bool> selector = (yearSub) => { return yearSub.Yyear == year; };
        return DbAccessUtilities.GetEntities(selector, db.YearSubs);
    }

    public IReadOnlyList<ContractFee> GetContractFees(int year, int month)
    {
        using var db = ConnectToDb();
        Func<ContractFee, bool> selector = (contractFee) => { return contractFee.Cfmonth == month && contractFee.Cfyear == year; };
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
            DbAccessUtilities.WriteExceptionToDebug(ex);
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
            DbAccessUtilities.WriteExceptionToDebug(ex);
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
            DbAccessUtilities.WriteExceptionToDebug(ex);
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
            DbAccessUtilities.WriteExceptionToDebug(ex);
            contractFee = new ContractFee();
        }

        return contractFee;
    }
    #endregion

}

