using Microsoft.Data.Sqlite;
using System.Data.Common;
//using System.Data.Entity;
using System.Diagnostics;

using DbAccess.Models;

namespace DatabaseAccess
{
    //public delegate AlmanDefinitions.ReturnCode ExectuteNonQueryTransaction()
    public class DbAccsess //: IAlmanDbAccess
    {

        
        public DbAccsess() { }

        private static AlmanContext ConnectToDb()
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


        #region Playgound, delete after final impl
  
        #endregion


        public ICollection<Child> GetChildren()
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
            using (var transaction = ctx.Database.BeginTransaction())
            {

            }
            ctx.Children.Add(new Child { ChildName = "name4", ChildLastName = "lastname4" });
            ctx.SaveChanges();

        }

        public void RemoveChild()
        {

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