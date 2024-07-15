using System.Collections.Generic;
using System.Collections;
using DatabaseAccess;
using DbAccess.Models;
using Microsoft.Extensions.Logging;

namespace business_log
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseAccess.DbChildren();

/*            var children = db.GetChildren();
            foreach ( var child in children )
            {
                child.YearMonthActivities.Add(new YearMonthActivity { YmactivityId = 1, YmactivitySum = 300, Month = 6, Year = 2024, YmwayOfPaying = (int)WayOfPaingForActivity.EveryLesson, YmwasPaid = 1 });
            }
            db.UpdateChildren( children );*/

/*            var childrenToDel = db.GetChildren(ch => ch.ChildId == 5);
            db.DeleteChildren(childrenToDel);*/

            //db.AddNewChildren(new Child[] {new Child { ChildName = "Name6", ChildLastName = "LastName6", ChildGroup = 2, ChildContract = (int)ContractType.MotherCapital},
            //                               new Child { ChildName = "Name7", ChildLastName = "LastName7", ChildGroup = 2, ChildContract = (int)ContractType.MotherCapital}});

            var children = db.GetChildren(ch => true);
            /* foreach (var child in children)
             {
                 child.YearMonthActivities.Remove(child.YearMonthActivities.First());
             }*/
            db.DeleteChildren(children);
        }
    }
}
