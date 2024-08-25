using Alman.SharedModels;
using DbAccess;
using Microsoft.EntityFrameworkCore;
namespace DbAccess.Models;

public partial class Precontract : IPrecontractBase, IDeleteDependable
{
    public int Id { get; set; }

    public int PchildId { get; set; }

    public int Psum { get; set; }

    public string? Pcomment { get; set; }

    public int? PYear { get; set; }
    public int? PMonth { get; set; }
    public virtual Child Pchild { get; set; } = null!;

    public void DeleteDependable(DbContext db) { }

}
