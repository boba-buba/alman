using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

public class StaffMemberUI : IStaffMemberBase
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    public int StartYear { get; set; }
    public int StartMonth { get; set; }
    public int State { get; set; }

    public string? PositionName { get; set; }

    public int PositionSalary { get; set; }
}
