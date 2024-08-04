using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

internal class PositionUI : IPositionBase
{
    public int PositionId { get; set; }

    public string? PositionName { get; set; }

    public int? PositionSalary { get; set; }
}
