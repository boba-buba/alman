namespace Alman.SharedModels;

public interface IStaffMemberBase : IIdentifier
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int StartYear { get; set; }
    public int StartMonth { get; set; }
    public int State { get; set; }
    public string? PositionName { get; set; }

    public int? PositionSalary { get; set; }
}