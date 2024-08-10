namespace Alman.SharedModels;

public interface IStaffMemberBase : IIdentifier
{
    //public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }


    public int State { get; set; }
    public string? PositionName { get; set; }

    public string? PositionSalary { get; set; }
}