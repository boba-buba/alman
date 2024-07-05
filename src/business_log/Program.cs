namespace business_log
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var children = DatabaseAccess.DbAccsess.GetChildren();
            foreach (var child in children)
            {
                Console.WriteLine(child);
            }
        }
    }
}
