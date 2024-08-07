﻿using Alman.SharedModels;
namespace DbAccess.Models;

public partial class Precontract : IPrecontractBase
{
    public int PchildId { get; set; }

    public int Psum { get; set; }

    public string? Pcomment { get; set; }

    public int? PYear { get; set; }
    public int? PMonth { get; set; }
    public virtual Child Pchild { get; set; } = null!;
}
