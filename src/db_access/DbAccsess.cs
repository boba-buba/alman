using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Data.Entity;
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

        private static void EndConnection(AlmanContext ctx)
        {
            ctx.Dispose();
        }

        public void ExecuteTransaction()
        {
            using var ctx = ConnectToDb();

            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                ctx.Children.Add(new Child { ChildName = "name3", ChildLastName = "lastname2" });
                
                ctx.SaveChanges();
                throw new Exception();
                transaction.Commit();
            } catch (Exception ex)
            {

                transaction.Rollback();
            }
        }

        private AlmanDefinitions.ReturnCode ExecuteNonQueryTransaction(AlmanContext ctx)
        {
            using (var transaction =  ctx.Database.BeginTransaction())
            {
                transaction.Commit();
            }

            return AlmanDefinitions.ReturnCode.OK;
        }
        public IEnumerable<Child> GetChildren()
        {
            var connection = ConnectToDb();
            IEnumerable<Child> children = connection.Children.ToArray();
            EndConnection(connection);
            return children;
        }

        public static void AddNewChild()
        {
            using var ctx = ConnectToDb();
            ctx.Children.Add(new Child { ChildName = "name3", ChildLastName = "lastname2" });

            ctx.SaveChanges();

        }

        public static void RemoveChild()
        {

        }

    }
}
