using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IExpenseBase : IIdentifier
{
    public string Name { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }

    public int ExpenseSum { get; set;}

    public int WayOfPaying { get; set; }

}
