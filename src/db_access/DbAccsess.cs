namespace DatabaseAccess
{
    public static class DbAccsess
    {
        private static AlmanContext ConnectToDb()
        {
            return new AlmanContext();
        }

        public static IEnumerable<Child> GetChildren()
        {
            var connection = ConnectToDb();
            IEnumerable<Child> children = connection.Children.ToArray();
            
            return children;
        }

        public static void AddNewChild()
        {

        }

        public static void RemoveChild()
        {

        }

    }
}
