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
            child.YearSubs.Single().YoctoberPayment = (int)AlmanDefinitions.WayOfPaying.Cash;
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



public class DbChildren : IAlmanChildrenRead
{
    #region HelperFunctions
    private void WriteExceptionToDebug(Exception exception)
    {
        Debug.WriteLine("----Another one----");
        Debug.Assert(exception != null);
        Debug.WriteLine(exception.ToString());
        Debug.WriteLine(exception.Message);
        Debug.WriteLine(exception.StackTrace);
        Debug.WriteLine("---------End--------");
    }
    #endregion
    public DbChildren() { }
    
    private AlmanContext ConnectToDb()
    {
        return new AlmanContext("C:\\Users\\ncoro\\source\\repos\\alman\\src\\db_access\\Database\\alman.db");
    }

    /*
    private IReadOnlyList<TEntity> GetEntities<TEntities, TEntity>(Func<TEntity, bool> predicate, TEntities entities, AlmanContext ctx) where TEntity : class where TEntities : DbSet<TEntity>
    {
        using var db = ConnectToDb();
        return entities.Where(predicate).ToList();
    }*/

    public IReadOnlyList<Child> GetChildren()
    {
        using var db = ConnectToDb();
        return db.Children.ToList();
    }

    /* Children activities list **/
    public IReadOnlyList<DbAccess.Models.Activity> GetActivities()
    {
        using var db = ConnectToDb();
        return db.Activities.ToList();
    }

    public IReadOnlyList<Precontract> GetPrecontracts(int year)
    {
        using var db = ConnectToDb();
        return db.Precontracts.ToList();
    }

    /* Year Month Table for the month of the year
     * 
     */
    public IReadOnlyList<YearMonthActivity> GetYearMonthActivities(int month, int year)
    {
        using var db = ConnectToDb();
        return db.YearMonthActivities.Where(ymActivity => ymActivity.Month == month && ymActivity.Year == year).ToList();
    }

    public IReadOnlyList<YearSub> GetYearSubs(int year)
    {
        using var db = ConnectToDb();
        return db.YearSubs.Where(ySub => ySub.Yyear == year).ToList();
    }

    public IReadOnlyList<ContractFee> GetContractFees(int year, int month)
    {
        using var db = ConnectToDb();
        List<ContractFee> contractFees;
        try
        {
            contractFees = db.ContractFees.Where(
                cFee => 
                cFee.Cfyear == year && 
                cFee.Cfmonth == month)
                .ToList();
        }catch(Exception ex)
        {
            WriteExceptionToDebug(ex);
            contractFees = new List<ContractFee>();
        }
        return contractFees;
    }

    public IReadOnlyList<Child> GetChildrenByName(string firstName, string lastName)
    {
        using var db = ConnectToDb();
        List<Child> children;
        try
        {
            children = db.Children.Where(
                child => 
                child.ChildName == firstName &&
                child.ChildLastName == lastName)
                .ToList();
        }
        catch (ArgumentNullException ex)
        {
            WriteExceptionToDebug(ex);
            children = new List<Child>();
        }
        return children;
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
            WriteExceptionToDebug(ex);
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
            precontract = db.Precontracts.Single(pr => pr.PchildId ==  ChildId);
        }
        catch (Exception ex)
        {
            WriteExceptionToDebug(ex);
            precontract = new Precontract();
        }
        return precontract;
    }

    /* Get all child's activities for month. */
    public IReadOnlyList<YearMonthActivity> GetYearMonthActivitiesById(int year, int month, int ChildId)
    {
        using var db = ConnectToDb();
        
        return db.YearMonthActivities.Where(
            ymActivity => 
            ymActivity.Year == year &&
            ymActivity.Month == month &&
            ymActivity.YmchildId == ChildId)
            .ToList();
    }

    public YearSub GetChildYearSubById(int year, int ChildId);

    public ContractFee GetContractFeeById(int year, int month, int ChildId);

