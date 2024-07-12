using System.Collections.Generic;
using System.Collections;
using DatabaseAccess;
using DbAccess.Models;


namespace business_log
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseAccess.DbChildren();
            //db.AddNewChild();
            /*            var children = db.GetChildren();

                        //var ch = db.GetChildById(6);
                        //db.DeleteChildren( new[] { ch });
                        //var li = new List<YearSub>();
                        foreach (var child in children)
                        {
                            //child.ChildContract = (int)AlmanDefinitions.ContractType.StaffChild;
                            child.YearSubs.Add(new DbAccess.Models.YearSub { Yyear = 2024, Yjune = 2000, YjunePayment = (int)AlmanDefinitions.WayOfPaying.Transfer });
                            //var yearSub = db.GetChildYearSubById(2025, child.ChildId);
                            //yearSub.Yyear = 2026;
                            //li.Add(yearSub);s
                        }*/

            var activities = db.GetYearMonthActivities(6, 2024);
            foreach ( var activity in activities )
            {
                Console.WriteLine(activity);
            }

            //db.UpdateYearSubsAsync(li);
        }
    }
}
