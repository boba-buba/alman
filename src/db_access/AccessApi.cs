using DatabaseAccess;


public interface AlmanDbAccess
{
    #region Children operations reading

    /* Children table*/
    public IEnumerable<Child> GetChildren();

    /** Year Month Table for the month of the year
     * 
     */
    public IEnumerable<YearMonthActivity> GetYearMonthACtivities(int month, int year);

    public IEnumerable<YearSub> GetYearSubs(int year);

    public IEnumerable<Precontract> GetPrecontracts(int year);

    /* Children activities list **/
    public IEnumerable<Activity> GetActivities();

    public Child GetChildByName(string FirstName, string LastName);

    public Child GetChildById(int Id);

    public YearSub GetChildYearSubById(int ChildId);

    public IEnumerable<YearMonthActivity> GetChildYearMonthActivitiesById(int ChildId);

    #endregion

    #region Children operations writing
    public AlmanDefinitions.ReturnCode AddNewChild(Child child);

    /** Set flag ChildState to false*/
    public AlmanDefinitions.ReturnCode DeleteChildById(int ChildId);

    #endregion
    public IEnumerable<OtherActivity> GetOtherActivities();
 
}
