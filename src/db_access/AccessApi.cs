using DatabaseAccess;
using DbAccess.Models;



//nekdy nejlip pouzit to filtrovani primo v sql dotazu, protoze temi where to muze z db natahnout spoustu dat, ktere budeme filtrovat zde. Radeji je primo vyfiltrujeme v dbazovem dotazu.
public interface IAlmanDbAccess
{
    // v impl lze pouzit linq
    #region Children operations reading

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


    public IEnumerable<YearMonthActivity> GetChildYearMonthActivitiesById(int ChildId);

    #endregion

    #region Children operations writing
    public AlmanDefinitions.ReturnCode AddNewChildren(IEnumerable<Child> children);
    public AlmanDefinitions.ReturnCode UpdateChildren(IEnumerable<Child> children);
    public AlmanDefinitions.ReturnCode DeleteChildren(IEnumerable<Child> children);
    public AlmanDefinitions.ReturnCode AddActvities(IEnumerable<Activity> actvities);
    public AlmanDefinitions.ReturnCode UpdateActvities(IEnumerable<Activity> updatedActvities);
    public AlmanDefinitions.ReturnCode DeleteActvities(IEnumerable<Activity> actvitiesToDelete);
    
    //ty tabulky s temi cizimi klicemi
    /**Set flag ChildState to false*/
    //public AlmanDefinitions.ReturnCode DeleteChildById(int ChildId);



    #endregion
    //public IEnumerable<OtherActivity> GetOtherActivities();
 
}

public interface IAlmanChildrenRead
{

}

public interface IAlmanChildrenWrite
{

}

public interface IAlman