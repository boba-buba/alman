namespace Alman.SharedModels;

public interface IChildBase : IIdentifier
{
    //public int Id { get; set; }

    public string ChildName { get; set; }

    public string ChildLastName { get; set; }

    public int ChildContract { get; set; }

    public int ChildGroup { get; set; }

    public int ChildState { get; set; }

    public int ChildStartYear { get; set; }

    public int ChildStartMonth { get; set; }
}