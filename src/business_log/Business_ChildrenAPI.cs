using Alman.Models;
using DbAccess.Models;

namespace Business;

public static class BusinessChildrenAPI
{
    public static IReadOnlyList<Child> GetChildren()
    {
        var db = new DbChildren();
        return db.GetChildren(ch => true);
    }
}
