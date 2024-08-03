namespace Alman.SharedModels;

public interface IPositionBase
{
    public int PositionId { get; set; }

    public string? PositionName { get; set; }

    public int? PositionSalary { get; set; }
}