using DatabaseAccess;
using DbAccess.Models;



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
    public ReturnCode AddNewChildren(IEnumerable<Child> children);
    public ReturnCode UpdateChildren(IEnumerable<Child> children);
    public ReturnCode DeleteChildren(IEnumerable<Child> children);

    public ReturnCode AddActvities(IEnumerable<Activity> activities);
    public ReturnCode UpdateActvities(IEnumerable<Activity> activities);
    public ReturnCode DeleteActvities(IEnumerable<Activity> activities);

    public ReturnCode AddPrecontract(IEnumerable<Precontract> precontract);
    public ReturnCode UpdatePrecontract(IEnumerable<Precontract> precontract);
    public ReturnCode DeletePrecontract(IEnumerable<Precontract> precontract);

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
    public IReadOnlyList<StaffMember> GetStaffMembers();
    public IReadOnlyList<Position> GetPositions();
    public IReadOnlyList<Prepayment> GetPrepaymentsForYearMonth(int month, int year);
    public IReadOnlyList<FinalPayment> GetFinalPaymentsForYearMonth(int month, int year);
    public IReadOnlyList<StaffActivity> GetStaffActivities();
    public IEnumerable<YearMonthStaffActivity> GetYearMonthStaffActivitiesById(int month, int year);

    public StaffMember GetStaffMemberById(int id);
    public IReadOnlyList<StaffMember> GetStaffMembersByName(string firstName, string lastName);
}


public interface IAlmanStaffMemeberWrite
{

}


public interface IAlmanOtherRead
{
    public IReadOnlyList<OtherActivity> GetOtherActivities();
    public IReadOnlyList<YearMonthOther> GetYearMonthOthers();

}

public interface IAlmanOtherWrite
{

}