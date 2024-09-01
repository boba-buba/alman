using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

/// <summary>
/// Database model for the expense entity model.
/// </summary>
public class Expense : IExpenseBase, IDeleteDependable
{
    public int Id { get; set; }
    
    public string Name { get; set; } = "";
    
    public int Month { get; set; }

    public int Year { get; set; }

    public int ExpenseSum { get; set; }

    public int WayOfPaying { get; set; }

    public void DeleteDependable(DbContext dbContext) { }
}
