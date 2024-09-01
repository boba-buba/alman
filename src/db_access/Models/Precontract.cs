using Alman.SharedModels;
using DbAccess;
using Microsoft.EntityFrameworkCore;
namespace DbAccess.Models;

/// <summary>
/// Database model for the precontract entity.
/// </summary>
public partial class Precontract : IPrecontractBase, IDeleteDependable
{
    public int Id { get; set; }

    public int PchildId { get; set; }

    public int Psum { get; set; }

    public string? Pcomment { get; set; }

    public int? PYear { get; set; }

    public int? PMonth { get; set; }

    /// <summary>
    /// The child to whom the precontract belongs .
    /// </summary>
    public virtual Child Pchild { get; set; } = null!;

    public void DeleteDependable(DbContext db) { }

}
