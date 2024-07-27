using Alman.SharedModels;
namespace AlmanUI.Models;


public partial class ActivityUI : IActivityBase
{
    public int ActivityId { get; set; }

    public string ActivityName { get; set; } = null!;

    public int ActivityPrice { get; set; }
}