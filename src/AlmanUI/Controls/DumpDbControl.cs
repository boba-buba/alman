using Business;
using System.Threading.Tasks;
namespace AlmanUI.Controls;

/// <summary>
/// Functionality to dump the databse to the Excel file.
/// </summary>
public static class DumpDbControl
{
    /// <summary>
    /// Starts task to dump the database and checks for the result filename.
    /// </summary>
    /// <returns>Task, which result is the name of the file where the dumped database is.</returns>
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
