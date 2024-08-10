namespace Alman.SharedModels;

public interface IPositionBase : IIdentifier
{
    //public int Id { get; set; }

    public string? PositionName { get; set; }

    public int? PositionSalary { get; set; }
}