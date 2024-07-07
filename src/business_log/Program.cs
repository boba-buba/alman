namespace business_log
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseAccess.DbAccsess();
            db.ExecuteTransaction();
           /* var children = db.GetChildren();
            foreach (var child in children)
            {
                Console.WriteLine(child);
            }*/
        }
    }
}
