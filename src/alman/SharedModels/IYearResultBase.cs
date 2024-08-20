using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IYearResultBase : IIdentifier
{
    public int Year { get; set; }
    public int YearRemainder { get; set; }
}
