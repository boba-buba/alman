using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IOtherActivityBase : IIdentifier
{
    //public int Id { get; set; }

    public string? OtherName { get; set; }
}
