using Alman.SharedModels;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
namespace DbAccess.Models;

public partial class YearSub : IYearSubBase, IDeleteDependable
{
    public int Id { get; set; }

    public int YchildId { get; set; }

    public int Yyear { get; set; }
    public int Month { get; set; }

    public int Payment { get; set; }

    public int WayOfaying { get; set; }

    public virtual Child Ychild { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext) { }
}
