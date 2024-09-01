using Alman.SharedModels;
using System.Collections.Generic;
namespace AlmanUI.Models;

/// <summary>
/// UI model of the Yearly subscription model.
/// </summary>
public class YearSubUI : IYearSubBase
{
    public int Id { get; set; }
    public int YchildId { get; set; }

    public int Yyear { get; set; }
    public int Month { get; set; }

    public int Payment { get; set; }

    public int WayOfaying { get; set; }

}

/// <summary>
/// Utility class for the YearSubsView.
/// </summary>
public class YearSubCompositeItem
{
    /// <summary>
    /// Child.
    /// </summary>
    public IChildBase YsChild { get; set; }

    /// <summary>
    /// All child's yearly subscriptions for the particular year.
    /// </summary>
    public List<IYearSubBase> YsYearSubscriptions { get; set; }

    /// <summary>
    /// ctor.
    /// </summary>
    public YearSubCompositeItem()
    {
        YsChild = new ChildUI();
        YsYearSubscriptions = new List<IYearSubBase>();
    }
}
