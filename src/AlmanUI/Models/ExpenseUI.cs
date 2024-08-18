using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

public class ExpenseUI : IExpenseBase
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Month { get; set; }
    public int Year { get; set; }

    public int ExpenseSum { get; set; }

    public int WayOfPaying { get; set; }
}
