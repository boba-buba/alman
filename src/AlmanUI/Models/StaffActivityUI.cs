using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

public class StaffActivityUI : IStaffActivityBase
{
    public int Id { get; set; }

    public string? ActivityName { get; set; }
}
