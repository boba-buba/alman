using Microsoft.Data.Sqlite;
using System.Data.Common;
//using System.Data.Entity;
using System.Diagnostics;

using DbAccess.Models;
using System.Security.AccessControl;

namespace DatabaseAccess
{
    //public delegate AlmanDefinitions.ReturnCode ExectuteNonQueryTransaction()
    public partial class DbAccsess //: IAlmanDbAccess
    {

        
        public DbAccsess() { }

        private AlmanContext ConnectToDb()
        {
            return new AlmanContext("full path");
        }

        /* Explicit end of connection
         *
         */
        private static void EndConnection(AlmanContext? ctx)
        {
            if (ctx != null)
            {
                ctx.Dispose();
            }
        }



        public IReadOnlyList<Child> GetChildren()
        {
            using var connection = ConnectToDb();

            var children = connection.Children.ToList();
            return children;
        }

        public AlmanDefinitions.ReturnCode UpdateChildren(IEnumerable<Child> children)
        {
            using var ctx = ConnectToDb();

            foreach (var child in children)
            {
                //var ch = ctx.Children.Where(x => x.ChildId == child.ChildId).Single();
                //ch.ChildContract = child.ChildContract;
                ctx.Children.Update(child);
            }
            ctx.SaveChanges();
            return AlmanDefinitions.ReturnCode.OK;
        }

        public void AddNewChild()
        {
            using var ctx = ConnectToDb();
            ctx.Children.Add(new Child { ChildName = "name4", ChildLastName = "lastname4" });
            ctx.SaveChanges();

        }

        public AlmanDefinitions.ReturnCode DeleteChildren(IEnumerable<Child> children)
        {
            using var ctx = ConnectToDb();
            ctx.Children.RemoveRange(children);
            ctx.SaveChanges();
            return AlmanDefinitions.ReturnCode.OK;
        }

        public Child GetChildById(int ChildId)
        {
            using var ctx = ConnectToDb();
            Child child = ctx.Children.Where(c => c.ChildId == ChildId).Single();
            return child;
        }

    }
}

/* Updating 
 * void UpdateItem(Item item) {
    using(var db = new ProductsContext()) {
        db.Items.Update(item);
        db.SaveChanges();
    }
}
**/