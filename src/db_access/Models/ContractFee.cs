﻿using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class ContractFee
{
    public int CfchildId { get; set; }

    public int Cfmonth { get; set; }

    public int Cfyear { get; set; }

    public int? CfsumPaid { get; set; }

    public virtual Child Cfchild { get; set; } = null!;
}
