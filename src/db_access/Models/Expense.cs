using Alman.SharedModels;
using DbAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Models;

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
