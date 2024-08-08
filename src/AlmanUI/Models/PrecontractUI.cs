using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

internal class PrecontractUI : IPrecontractBase
{
    public int PchildId { get; set; }

    public int Psum { get; set; }

    public string? Pcomment { get; set; }

    public int? PYear { get; set; }

    public int? PMonth { get; set; }
}

public class PrecontractCompositeItem
{
    public IChildBase? PChild { get; set; }
    public IPrecontractBase? Precontract { get; set; }
}

