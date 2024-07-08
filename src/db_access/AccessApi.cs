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
    public IEnumerable<Child> GetChildren();

    /* Children activities list **/
    public IEnumerable<Activity> GetActivities();

    public IEnumerable<Precontract> GetPrecontracts(int year);

    /* Year Month Table for the month of the year
     * 
     */
    public IEnumerable<YearMonthActivity> GetYearMonthActivities(int month, int year);

    public IEnumerable<YearSub> GetYearSubs(int year);

    public IEnumerable<ContractFee> GetContractFees(int year);

    public IEnumerable<Child> GetChildrenByName(string FirstName, string LastName);

    public Child GetChildById(int ChildId);
    public Precontract GetPrecontractById(int ChildId);

    /* Get all child's activities for month.
     * 
     */
    public IEnumerable<YearMonthActivity> GetYearMonthActivitiesById(int year, int month, int ChildId);

    public YearSub GetChildYearSubById(int year, int ChildId);

    public ContractFee GetContractFeeById(int ChildId);

}


public interface IAlmanChildrenWrite
{
    public AlmanDefinitions.ReturnCode AddNewChildren(IEnumerable<Child> children);
    public AlmanDefinitions.ReturnCode UpdateChildren(IEnumerable<Child> children);
    public AlmanDefinitions.ReturnCode DeleteChildren(IEnumerable<Child> children);
    public AlmanDefinitions.ReturnCode AddActvities(IEnumerable<Activity> actvities);
    public AlmanDefinitions.ReturnCode UpdateActvities(IEnumerable<Activity> updatedActvities);
    public AlmanDefinitions.ReturnCode DeleteActvities(IEnumerable<Activity> actvitiesToDelete);

    // teoreticky jelikoz je vsechno vazano na dite, nebo aktivitu, tak muzu ty entity menit skrz dite, aktivitu a proto nepotrebuju operace na ty tabulky s cizimi klici??
    // podle potreby doplnit
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