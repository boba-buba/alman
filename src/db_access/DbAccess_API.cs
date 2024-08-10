using DatabaseAccess;
using DbAccess.Models;
using Alman.SharedDefinitions;



//nekdy nejlip pouzit to filtrovani primo v sql dotazu, protoze temi where to muze z db natahnout spoustu dat, ktere budeme filtrovat zde. Radeji je primo vyfiltrujeme v dbazovem dotazu.





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