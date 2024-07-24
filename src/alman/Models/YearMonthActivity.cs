using System;
using System.Collections.Generic;

namespace Alman.Models;

public partial class YearMonthActivity
{
    public int YmchildId { get; set; }

    public int YmactivityId { get; set; }

    public int YmactivitySum { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int YmwayOfPaying { get; set; }

    public int YmwasPaid { get; set; }

    public virtual Activity Ymactivity { get; set; } = null!;

    public virtual Child Ymchild { get; set; } = null!;
}
