using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace AlmanUI.Controls;

/// <summary>
/// API for managing the data in YearMonthActivitiesViewModel.
/// </summary>
public class YearMonthActivitiesControl : ControlBase<YearMonthActivity, IYearMonthActivityBase>
{
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <param name="year">Year for which data are saved.</param>
    /// <param name="month">Month for which the data are saved.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>
    public static ReturnCode SaveItems(IReadOnlyList<IYearMonthActivityBase> itemsToSave, int year, int month)
    {
        ReturnCode retCode = ReturnCode.OK;
        var ymActivitiesFromDb = GetItemsByFilter(act => act.Year == year && act.Month == month);
        int dbCount = ymActivitiesFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedYMActivities = itemsToSave.Where(ymAct => ymAct.InGroupId(ymActivitiesFromDb)).ToList();

            retCode = UpdateItems(updatedYMActivities);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IYearMonthActivityBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newYMActivities = itemsToSave.Where(ymAct => !ymAct.InGroupId(ymActivitiesFromDb)).ToList();
            retCode = AddItems(newYMActivities);
        }

        return retCode;
    }

    /// <summary>
    /// Calculate the child bill for the month.
    /// </summary>
    /// <param name="childId"> Id of the child to calcultae the bill for.</param>
    /// <param name="year">Year for which the data for the calculation are collected.</param>
    /// <param name="month">Month for which the data for the calculation are collected.</param>
    /// <returns></returns>
    public static ChildBill CalculateChildBill(int childId, int year, int month)
    {
        ChildBill bill = new ChildBill();
        bill.ChildId = childId;
        bill.Month = month;
        bill.Year = year;
        var activities = YearMonthActivitiesControl.GetItemsByFilter(act => act.YmchildId == childId && act.Year == year && act.Month == month);
        int activitiesSum = activities.Sum(act => act.YmactivitySum);
        bill.ActivitiesSum = activitiesSum;
        var precontracts = PrecontractsControl.GetItemsByFilter(pr => pr.PchildId == childId && pr.PMonth == month &&  pr.PYear == year);
        if (precontracts.Count == 1)
        {
            bill.PrecontractSum = precontracts[0].Psum;
        }
        var yearSubs = YearSubsControl.GetItemsByFilter(ys => ys.YchildId == childId && ys.Month == month && ys.Yyear == year);
        bill.YearSubsSum = yearSubs.Sum(sub => sub.Payment);
        var contractFees = ContractFeesControl.GetItemsByFilter(cf => cf.CfchildId == childId && cf.Cfmonth == month &&  cf.Cfyear == year);
        bill.ContractFeeSum = contractFees.Sum(cf => cf.CfsumPaid);
        foreach (var activity in activities)
        {
            var act = ActivitiesControl.GetItemById(activity.YmactivityId);
            bill.MonthlyActivities.Add(new ActivityMonth {ActivityName = act!.ActivityName, MonthlySum = activity.YmactivitySum});
        }
        return bill;
    }

}


/// <summary>
/// Utility child bill structure to get all childs expenses together.
/// </summary>
public struct ChildBill
{
    /// <summary>
    /// Id of the child.
    /// </summary>
    public int ChildId { get; set; } = 0;

    /// <summary>
    /// Year the bill is for.
    /// </summary>
    public int Year { get; set; } = 0;

    /// <summary>
    /// Month the bill is for.
    /// </summary>
    public int Month { get; set; } = 0;

    /// <summary>
    /// Overall sum for all activities the child took part in.
    /// </summary>
    public int ActivitiesSum { get; set; } = 0;

    /// <summary>
    /// Sum for the Precontract.
    /// </summary>
    public int PrecontractSum { get; set; } = 0;

    /// <summary>
    /// Sum for the yearly subscriptions.
    /// </summary>
    public int YearSubsSum { get; set; } = 0;

    /// <summary>
    /// Sum for the contract fees.
    /// </summary>
    public int ContractFeeSum { get; set; } = 0;

    /// <summary>
    /// All pairs activity-sum for it the child must pay for.
    /// </summary>
    public List<ActivityMonth> MonthlyActivities { get; set; }

    /// <summary>
    /// ctor.
    /// </summary>
    public ChildBill()
    {
        MonthlyActivities = new List<ActivityMonth>();
    }
}


/// <summary>
/// Utility structure that stores the name of the activity and the sum that was spent on the activity.
/// </summary>
public struct ActivityMonth
{
    /// <summary>
    /// Name of the activity.
    /// </summary>
    public string ActivityName { get; set; } = "";

    /// <summary>
    /// Sum that msut e paid for the activity.
    /// </summary>
    public int MonthlySum { get; set; } = 0;

    /// <summary>
    /// ctor.
    /// </summary>
    public ActivityMonth()
    {

    }
}

public static class YearMonthActivitiesExtensions
{
    public static bool DbEquals(this IYearMonthActivityBase item,  IYearMonthActivityBase other)
    {
        if (item.YmchildId !=  other.YmchildId) 
            return false;
        if (item.YmactivityId != other.YmactivityId) 
            return false;
        if (item.Month !=  other.Month) 
            return false;
        if (item.Year != other.Year) 
            return false;
        return true;
    }

    public static bool InGroup(this IYearMonthActivityBase item, IReadOnlyCollection<IYearMonthActivityBase> group)
    {
        if (group.Count == 0) { return false; }
        var itemInGroup = group.SingleOrDefault(cf => cf.DbEquals(item));
        if (itemInGroup == null) { return false; }
        return true;
    }
}