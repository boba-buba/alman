namespace Alman.SharedModels;

public interface IStaffMemberBase : IIdentifier
{
    //public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? PositionId { get; set; }

    public int State { get; set; }
}