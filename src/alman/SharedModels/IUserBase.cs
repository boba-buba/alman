using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IUserBase : IIdentifier
{
    public string Name { get; set; }
    public string Password { get; set; }

    public int Permissions { get; set; }

}
