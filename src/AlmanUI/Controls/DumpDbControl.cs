using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business;
namespace AlmanUI.Controls;

public static class DumpDbControl
{
    public static async Task<string> DumpDbAsync()
    {
        var res = await BusinessDbDump.DumpDb();
        string name = "";
        if (res is not null)
        {
            name = res;
        }
        return name;
    }

}
