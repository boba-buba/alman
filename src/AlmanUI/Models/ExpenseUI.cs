using Alman.SharedModels;

namespace AlmanUI.Models;

/// <summary>
/// UI model of the Expense entity.
/// </summary>
public class ExpenseUI : IExpenseBase
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Month { get; set; }
    public int Year { get; set; }

    public int ExpenseSum { get; set; }

    public int WayOfPaying { get; set; }
}
