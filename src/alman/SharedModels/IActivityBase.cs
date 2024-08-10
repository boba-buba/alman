
namespace Alman.SharedModels;
public interface IIdentifier
{
    public int Id { get; set; }

}

public interface IActivityBase : IIdentifier
{
    public int Id { get; set; }

    public string ActivityName { get; set; }

    public int ActivityPrice { get; set; }
}