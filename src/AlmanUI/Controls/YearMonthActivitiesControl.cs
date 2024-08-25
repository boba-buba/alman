using Alman.SharedModels;
using Alman.SharedDefinitions;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using DbAccess.Models;
using AlmanUI.Controls;
namespace AlmanUI.Controls;


public class YearMonthActivitiesControl : ControlBase<YearMonthActivity, IYearMonthActivityBase>
{
    
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

    public static ChildBill ComputeChildBill(int childId, int year, int month)
    {
        ChildBill bill = new ChildBill();
        bill.ChildId = childId;
        bill.Month = month;
        bill.Year = year;
        var activities = YearMonthActivitiesControl.GetItemsByFilter(act => act.YmchildId == childId && act.Year == year && act.Month == month);
        int activitiesSum = activities.Sum(act => act.YmactivitySum);
        bill.ActivitesSum = activitiesSum;
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
            bill.MonthlyActivities.Add(new ActivityMonth {ActivityName = ActivitiesControl.GetItemById(activity.Id)!.ActivityName, MonthlySum = activity.YmactivitySum});
        }
        return bill;
    }

}



public struct ChildBill
{
    public int ChildId { get; set; } = 0;
    public int Year { get; set; } = 0;
    public int Month { get; set; } = 0;
    public int ActivitesSum { get; set; } = 0;
    public int PrecontractSum { get; set; } = 0;
    public int YearSubsSum { get; set; } = 0;
    public int ContractFeeSum { get; set; } = 0;

    public List<ActivityMonth> MonthlyActivities { get; set; }
    public ChildBill()
    {
        MonthlyActivities = new List<ActivityMonth>();
    }
}

public struct ActivityMonth
{
    public string ActivityName { get; set; } = "";
    public int MonthlySum { get; set; } = 0;
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