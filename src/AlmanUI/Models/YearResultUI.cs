using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alman.SharedModels;

namespace AlmanUI.Models;

public class YearResultUI : IYearResultBase
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int YearRemainder { get; set; }
}
