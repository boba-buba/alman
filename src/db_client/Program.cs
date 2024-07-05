using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace db_client
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AlmanContext())
            {
                //db.Database.Migrate();
                Console.WriteLine($"Database path: {db.DbPath}.");


                //Console.WriteLine("Creating new child");
                //db.Add(new Child { ChildName = "кнмапп", ChildLastName = "йукпп", ChildContract = "апроо", ChildGroup = 1 });
                //db.SaveChanges();

                /*                Console.WriteLine("Adding precontract");
                                db.Add(new Precontract { PChildID = 1, PSum = 20000 });
                                db.SaveChanges();*/

                Console.WriteLine("Updating child group");
                Child? ch = db.Children.SingleOrDefault(c => c.ChildId == 1);
                if (ch != null )
                {
                    ch.ChildGroup = 3;
                    db.SaveChanges();
                }
                
                

            }
        }
    }
}
