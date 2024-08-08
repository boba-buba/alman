using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IYearSubBase
{
    public int YchildId { get; set; }

    public int Yyear { get; set; }

    public int Yjanuary { get; set; }

    public int YjanuaryPayment { get; set; }

    public int Yfebruary { get; set; }

    public int YfebruaryPayment { get; set; }

    public int Ymarch { get; set; }

    public int YmarchPayment { get; set; }

    public int Yapril { get; set; }

    public int YaprilPayment { get; set; }

    public int Ymay { get; set; }

    public int YmayPayment { get; set; }

    public int Yjune { get; set; }

    public int YjunePayment { get; set; }

    public int Yjuly { get; set; }

    public int YjulyPayment { get; set; }

    public int Yaugust { get; set; }

    public int YaugustPayment { get; set; }

    public int Yseptember { get; set; }

    public int YseptemberPayment { get; set; }

    public int Yoctober { get; set; }

    public int YoctoberPayment { get; set; }

    public int Ynovember { get; set; }

    public int YnovemberPayment { get; set; }

    public int Ydecember { get; set; }

    public int YdecemberPayment { get; set; }

}
