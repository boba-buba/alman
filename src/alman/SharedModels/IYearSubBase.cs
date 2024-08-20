using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IYearSubBase : IIdentifier
{
    public int YchildId { get; set; }

    public int Yyear { get; set; }

    public int Month { get; set; }

    public int Payment { get; set; }

    public int WayOfaying { get; set; } 
}
