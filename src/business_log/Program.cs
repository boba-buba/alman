namespace business_log
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseAccess.DbAccsess();
            var children = db.GetChildren();
            foreach (var child in children)
            {
                Console.WriteLine(child);
            }
        }
    }
}
