using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IYearMonthStaffActivityBase : IIdentifier
{
   
    public int StaffMemberId { get; set; }

    public int StaffActivityId { get; set; }

    public decimal? SumPaid { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int WasPaid {  get; set; }

}
