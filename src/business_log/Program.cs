using DatabaseAccess;

namespace business_log
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseAccess.DbAccsess();
            db.AddNewChild();
            var children = db.GetChildren();

            foreach ( var child in children )
            {
                child.ChildContract = (int)AlmanDefinitions.ContractType.StaffChild;
            }
            db.UpdateChildren(children);

        }
    }
}
