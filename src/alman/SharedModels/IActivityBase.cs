namespace Alman.SharedModels;

public interface IActivityBase
{
    public int ActivityId { get; set; }

    public string ActivityName { get; set; }

    public int ActivityPrice { get; set; }
}