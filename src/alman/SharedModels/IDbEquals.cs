using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IDbEquals
{
    public static abstract bool DbEquals(IDbEquals other);
}
