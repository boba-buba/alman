using System;
using System.Collections.Generic;
using Alman.SharedModels;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
namespace DbAccess.Models;

public partial class Position : IPositionBase, IDeleteDependable
{
    public int Id { get; set; }

    public string? PositionName { get; set; }

    public int? PositionSalary { get; set; }

    public virtual ICollection<StaffMember> StaffMembers { get; set; } = new List<StaffMember>();

    public void DeleteDependable(DbContext dbContext)
    {
        AlmanContext ctx = (AlmanContext)dbContext;
        List<StaffMember> staffMembers = ctx.StaffMembers.Where(staffMember => staffMember.PositionId == Id).ToList();
        DbAccessUtilities.DeleteEntities(staffMembers, ctx);
    }
}
