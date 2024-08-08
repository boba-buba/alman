using DatabaseAccess;
using DbAccess.Models;
using Alman.SharedDefinitions;


//nekdy nejlip pouzit to filtrovani primo v sql dotazu, protoze temi where to muze z db natahnout spoustu dat, ktere budeme filtrovat zde. Radeji je primo vyfiltrujeme v dbazovem dotazu.


public interface IAlmanChildrenRead
{

    /* Children table*/
    public IReadOnlyList<Child> GetChildren(Func<Child, bool> selector);

    /* Children activities list **/
    public IReadOnlyList<Activity> GetActivities(Func<Activity, bool> selector);

    public IReadOnlyList<Precontract> GetPrecontracts(Func<Precontract, bool> selector);

    /* Year Month Table for the month of the year
     * 
     */
    public IReadOnlyList<YearMonthActivity> GetYearMonthActivities(Func<YearMonthActivity, bool> selector);

    public IReadOnlyList<YearSub> GetYearSubs(Func<YearSub, bool> selector);

    public IReadOnlyList<ContractFee> GetContractFees(Func<ContractFee, bool> selector);

    public IReadOnlyList<Child> GetChildrenByName(string firstName, string lastName);

    public Child GetChildById(int ChildId);


    public Precontract GetPrecontractById(int ChildId);
    /* Get all child's activities for month. */
    public IReadOnlyList<YearMonthActivity> GetYearMonthActivitiesById(int year, int month, int ChildId);

    public YearSub GetChildYearSubById(int year, int ChildId);

    public ContractFee GetContractFeeById(int year, int month, int ChildId);
}


public interface IAlmanChildrenWrite
{
    public ReturnCode AddChildren(IEnumerable<Child> children);
    public ReturnCode UpdateChildren(IEnumerable<Child> children);
    public ReturnCode DeleteChildren(IEnumerable<Child> children);

    public ReturnCode AddActvities(IEnumerable<Activity> activities);
    public ReturnCode UpdateActvities(IEnumerable<Activity> activities);
    public ReturnCode DeleteActvities(IEnumerable<Activity> activities);

    public ReturnCode AddPrecontracts(IEnumerable<Precontract> precontract);
    public ReturnCode UpdatePrecontracts(IEnumerable<Precontract> precontract);
    public ReturnCode DeletePrecontracts(IEnumerable<Precontract> precontract);

    public ReturnCode AddYearMonthActivities(IEnumerable<YearMonthActivity> yearMonthActivities);
    public ReturnCode UpdateYearMonthActivities(IEnumerable<YearMonthActivity> yearMonthActivities);
    public ReturnCode DeleteYearMonthActivities(IEnumerable<YearMonthActivity> yearMonthActivities);

    public ReturnCode AddYearSubs(IEnumerable<YearSub> yearSubs);
    public ReturnCode UpdateYearSubs(IEnumerable<YearSub> yearSubs);
    public ReturnCode DeleteYearSubs(IEnumerable<YearSub> yearSubs);

    public ReturnCode AddContractFees(IEnumerable<ContractFee> contractFees);
    public ReturnCode UpdateContractFees(IEnumerable<ContractFee> contractFees);
    public ReturnCode DeleteContractFees(IEnumerable<ContractFee> contractFees);
}


public interface IAlmanStaffRead
{
    public IReadOnlyList<StaffMember> GetStaffMembers(Func<StaffMember, bool> selector);
    public IReadOnlyList<Position> GetPositions(Func<Position, bool> selector);
    public IReadOnlyList<Prepayment> GetPrepaymentsForYearMonth(Func<Prepayment, bool> selector);
    public IReadOnlyList<FinalPayment> GetFinalPaymentsForYearMonth(Func<FinalPayment, bool> selector);
    public IReadOnlyList<StaffActivity> GetStaffActivities(Func<StaffActivity, bool> selector);
    public IEnumerable<YearMonthStaffActivity> GetYearMonthStaffActivitiesById(Func<YearMonthStaffActivity, bool> selector);
    public StaffMember GetStaffMemberById(int id);
    public IReadOnlyList<StaffMember> GetStaffMembersByName(string firstName, string lastName);
}


public interface IAlmanStaffWrite
{
    public ReturnCode AddStaffMembers(IEnumerable<StaffMember> members);
    public ReturnCode UpdateStaffMembers(IEnumerable<StaffMember> members);
    public ReturnCode DeleteStaffMembers(IEnumerable<StaffMember> members);

    public ReturnCode AddPositions(IEnumerable<Position> positions);
    public ReturnCode UpdatePositions(IEnumerable<Position> positions);
    public ReturnCode DeletePositions(IEnumerable<Position> positions);

    public ReturnCode AddPrepayments(IEnumerable<Prepayment> prepayments);
    public ReturnCode UpdatePrepayments(IEnumerable<Prepayment> prepayments);
    public ReturnCode DeletePrepayments(IEnumerable<Prepayment> prepayments);

    public ReturnCode AddFinalPayments(IEnumerable<FinalPayment> finalPayments);
    public ReturnCode UpdateFinalPayments(IEnumerable<FinalPayment> finalPayments);
    public ReturnCode DeleteFinalPayments(IEnumerable<FinalPayment> finalPayments);

    public ReturnCode AddStaffActivities(IEnumerable<StaffActivity> activities);
    public ReturnCode UpdateStaffActivity(IEnumerable<StaffActivity> activities);
    public ReturnCode DeleteStaffActivities(IEnumerable<StaffActivity> activities);

    public ReturnCode AddYearMonthStaffActivities(IEnumerable<YearMonthStaffActivity> activities);
    public ReturnCode UpdateYearMonthStaffActivities(IEnumerable<YearMonthStaffActivity> activities);
    public ReturnCode DeleteYearMonthStaffActivities(IEnumerable<YearMonthStaffActivity> activities);

}


public interface IAlmanOtherRead
{
    public IReadOnlyList<OtherActivity> GetOtherActivities(Func<OtherActivity, bool> selector);
    public IReadOnlyList<YearMonthOther> GetYearMonthOthers(Func<YearMonthOther, bool> selector);

}

public interface IAlmanOtherWrite
{
    public ReturnCode AddOtherActivities(IEnumerable<OtherActivity> activities);
    public ReturnCode UpdateOtherActivities(IEnumerable<OtherActivity> activities);
    public ReturnCode DeleteOtherActivities(IEnumerable<OtherActivity> activities);

    public ReturnCode AddYearMonthOthers(IEnumerable<YearMonthOther> others);
    public ReturnCode UpdateYearMonthOthers(IEnumerable<YearMonthOther> others);
    public ReturnCode DeleteYearMonthOthers(IEnumerable<YearMonthOther> others);
}