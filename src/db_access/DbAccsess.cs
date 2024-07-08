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
            return new AlmanContext();
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


        public IEnumerable<Child> GetChildren()
        {
            using var connection = ConnectToDb();

            IEnumerable<Child> children = connection.Children.ToList();
            return children;
        }

        public static void AddNewChild()
        {
            using var ctx = ConnectToDb();
            using (var transaction = ctx.Database.BeginTransaction())
            {

            }
            ctx.Children.Add(new Child { ChildName = "name3", ChildLastName = "lastname2" });
            ctx.SaveChanges();

        }

        public static void RemoveChild()
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