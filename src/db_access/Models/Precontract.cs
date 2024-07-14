using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class Precontract
{
    public int PchildId { get; set; }

    public int Psum { get; set; }

    public string? Pcomment { get; set; }

    public virtual Child Pchild { get; set; } = null!;
}
