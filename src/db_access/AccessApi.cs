using DatabaseAccess;
using DbAccess.Models;

public interface IAlmanDbAccess
{
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
    //public AlmanDefinitions.ReturnCode AddNewChild(Child child);

    /**Set flag ChildState to false*/
    //public AlmanDefinitions.ReturnCode DeleteChildById(int ChildId);



    #endregion
    //public IEnumerable<OtherActivity> GetOtherActivities();
 
}
