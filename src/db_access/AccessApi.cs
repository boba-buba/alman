using DatabaseAccess;
using DbAccess.Models;



//nekdy nejlip pouzit to filtrovani primo v sql dotazu, protoze temi where to muze z db natahnout spoustu dat, ktere budeme filtrovat zde. Radeji je primo vyfiltrujeme v dbazovem dotazu.
public interface IAlmanDbAccess
{
    // v impl lze pouzit linq
    #region Children operations reading

    #endregion

    #region Children operations writing
    

    //ty tabulky s temi cizimi klicemi
    /** Set flag ChildState to false */
    //public AlmanDefinitions.ReturnCode DeleteChildById(int ChildId);



    #endregion
    //public IEnumerable<OtherActivity> GetOtherActivities();

}


public interface IAlmanChildrenRead
{

    /* Children table*/
    public IReadOnlyList<Child> GetChildren();

    /* Children activities list **/
    public IReadOnlyList<Activity> GetActivities();

    public IReadOnlyList<Precontract> GetPrecontracts(int year);

    /* Year Month Table for the month of the year
     * 
     */
    public IReadOnlyList<YearMonthActivity> GetYearMonthActivities(int month, int year);

    public IReadOnlyList<YearSub> GetYearSubs(int year);

    public IReadOnlyList<ContractFee> GetContractFees(int year, int month);

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
    public AlmanDefinitions.ReturnCode AddNewChildren(IEnumerable<Child> children);
    public AlmanDefinitions.ReturnCode UpdateChildren(IEnumerable<Child> children);
    public AlmanDefinitions.ReturnCode DeleteChildren(IEnumerable<Child> children);

    public AlmanDefinitions.ReturnCode AddActvities(IEnumerable<Activity> actvities);
    public AlmanDefinitions.ReturnCode UpdateActvities(IEnumerable<Activity> updatedActvities);
    public AlmanDefinitions.ReturnCode DeleteActvities(IEnumerable<Activity> actvitiesToDelete);

    public AlmanDefinitions.ReturnCode AddPrecontract(Precontract precontract);
    public AlmanDefinitions.ReturnCode UpdatePrecontract(Precontract precontract);
    public AlmanDefinitions.ReturnCode DeletePrecontract(Precontract precontract);

    public AlmanDefinitions.ReturnCode AddYearMonthActivities(IEnumerable<YearMonthActivity> yearMonthActivities);
    public AlmanDefinitions.ReturnCode UpdateYearMonthActivities(IEnumerable<YearMonthActivity> yearMonthActivities);
    public AlmanDefinitions.ReturnCode DeleteYearMonthActivities(IEnumerable<YearMonthActivity> yearMonthActivities);

    public AlmanDefinitions.ReturnCode AddYearSubs(IEnumerable<YearSub> yearSub);
    public AlmanDefinitions.ReturnCode UpdateYearSubs(IEnumerable<YearSub> yearSub);
    public AlmanDefinitions.ReturnCode DeleteYearSubs(IEnumerable<YearSub> yearSub);

    public AlmanDefinitions.ReturnCode AddContractFees(IEnumerable<ContractFee> contractFees);
    public AlmanDefinitions.ReturnCode UpdateContractFees(IEnumerable<ContractFee> contractFees);
    public AlmanDefinitions.ReturnCode DeleteContractFees(IEnumerable<ContractFee> contractFees);
}


public interface IAlmanStaffMemberRead
{
    public IEnumerable<StaffMember> GetStaffMembers();
    public IEnumerable<Position> GetPositions();
    public IEnumerable<Prepayment> GetPrepaymentsForYearMonth(int month, int year);
    public IEnumerable<FinalPayment> GetFinalPaymentsForYearMonth(int month, int year);
    public IEnumerable<StaffActivity> GetStaffActivities();
    public IEnumerable<YearMonthStaffActivity> GetYearMonthStaffActivitiesById(int month, int year);
}


public interface IAlmanStaffMemeberWrite
{

}


public interface IAlmanOtherRead
{

}

public interface IAlmanOtherWrite
{

}