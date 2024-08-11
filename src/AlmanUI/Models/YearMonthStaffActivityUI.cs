using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

public class YearMonthStaffActivityUI : IYearMonthStaffActivityBase
{
    public int Id { get; set; }

    public int StaffMemberId { get; set; }

    public int StaffActivityId { get; set; }

    public decimal? SumPaid { get; set; }

    public int WasPaid {  get; set; }
    public int Month { get; set; }

    public int Year { get; set; }

}

public class YearMonthStaffActivityCompositeItem
{
    public IStaffMemberBase? StaffMember { get; set; }
    public IList<IYearMonthStaffActivityBase>? Activities {get; set;}
}
