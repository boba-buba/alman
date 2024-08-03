namespace Alman.SharedModels;

public interface IStaffMemberBase
{
    public int StaffMemberId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? PositionId { get; set; }

    public int State { get; set; }
}