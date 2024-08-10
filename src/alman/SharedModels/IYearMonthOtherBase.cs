using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IYearMonthOtherBase : IIdentifier
{
    public int Id { get; set; }

    public int OtherActivityId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int? FirstWeek { get; set; }

    public int? SecondWeek { get; set; }

    public int? ThirdWeek { get; set; }

    public int? FourthWeek { get; set; }

    public int? FifthWeek { get; set; }

}
