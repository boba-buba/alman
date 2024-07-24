using System;
using System.Collections.Generic;

namespace Alman.Models;

public partial class OtherActivity
{
    public int OtherId { get; set; }

    public string? OtherName { get; set; }

    public virtual ICollection<YearMonthOther> YearMonthOthers { get; set; } = new List<YearMonthOther>();
}
