﻿using System;
using System.Collections.Generic;
using Alman.SharedModels;
namespace DbAccess.Models;

public partial class Position : IPositionBase
{
    public int PositionId { get; set; }

    public string? PositionName { get; set; }

    public int? PositionSalary { get; set; }

    public virtual ICollection<StaffMember> StaffMembers { get; set; } = new List<StaffMember>();
}
