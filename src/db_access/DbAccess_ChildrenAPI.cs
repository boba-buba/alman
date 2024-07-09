using Microsoft.Data.Sqlite;
using System.Data.Common;
//using System.Data.Entity;
using System.Diagnostics;

using DbAccess.Models;
using System.Security.AccessControl;

namespace DatabaseAccess;

public partial class DbAccsess
{
    public YearSub GetChildYearSubById(int year, int ChildId)
    {
        using var ctx = ConnectToDb();
        return ctx.YearSubs.Where(ys => ys.Yyear == year && ys.YchildId == ChildId).Single();
    }


    public AlmanDefinitions.ReturnCode UpdateYearSubs(IEnumerable<YearSub> yearSubs)
    {
        using var ctx = ConnectToDb();
        
        foreach (var yearSub in yearSubs)
        {
            //var dbYearSub = ctx.YearSubs.Where(ys => ys.YchildId == yearSub.YchildId && ys.Yyear == yearSub.Yyear).Single();
            ctx.YearSubs.Update(yearSub);
        }
        ctx.SaveChanges();
        return AlmanDefinitions.ReturnCode.OK;
    }

}