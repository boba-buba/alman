﻿using Alman.SharedModels;
namespace AlmanUI.Models;

public class YearMonthActivityUI : IYearMonthActivityBase
{
    public int YmchildId { get; set; }

    public int YmactivityId { get; set; }

    public int YmactivitySum { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int YmwayOfPaying { get; set; }

    public int YmwasPaid { get; set; }

}