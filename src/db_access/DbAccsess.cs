using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Data.Entity;

namespace DatabaseAccess
{
    public class DbAccsess // : AlmanDbAccess
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

        private void ExecuteTransaction()
        {
            var ctx = ConnectToDb();

            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                
                transaction.Commit();
            } catch (Exception ex)
            {
                transaction.Rollback();
            }
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

        }

        public static void RemoveChild()
        {

        }

        public static void CreateNewMonth(string month, string year)
        {
            var connection = ConnectToDb();
            
            SqliteConnection conn = new SqliteConnection($"Data Source={connection.DbPath}");
            conn.Open();
            SqliteCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"""
                    create table {month}_{year}_Activity (
                    YMChildId INTEGER,
                    YMActivityId INTEGER,
                    YMActivitySum INTEGER NOT NULL,
                	FOREIGN KEY(YMChildId) references Children(ChildId),
                	FOREIGN KEY(YMActivityId) references Activities(ActivityId),
                    Constraint PK_Year_Month_Act PRIMARY KEY (YMChildId, YMActivityId, YMActivitySum)
                );
                """;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }

    }
}
