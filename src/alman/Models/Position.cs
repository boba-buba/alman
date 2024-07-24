using System;
using System.Collections.Generic;

namespace Alman.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string? PositionName { get; set; }

    public int? PositionSalary { get; set; }

    public virtual ICollection<StaffMember> StaffMembers { get; set; } = new List<StaffMember>();
}
