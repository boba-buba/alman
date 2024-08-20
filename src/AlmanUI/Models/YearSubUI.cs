using Alman.SharedModels;
using System.Collections.Generic;
namespace AlmanUI.Models;

public class YearSubUI : IYearSubBase
{
    public int Id { get; set; }
    public int YchildId { get; set; }

    public int Yyear { get; set; }
    public int Month { get; set; }

    public int Payment { get; set; }

    public int WayOfaying { get; set; }

}


public class YearSubCompositeItem
{
    public IChildBase YsChild { get; set; }
    public List<IYearSubBase> YsYearSubscriptions { get; set; }

    public YearSubCompositeItem()
    {
        YsChild = new ChildUI();
        YsYearSubscriptions = new List<IYearSubBase>();
    }
}
