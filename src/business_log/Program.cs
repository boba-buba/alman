using System.Collections.Generic;
using System.Collections;
using DbAccess.Models;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Business
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*var dbName = "ChangePrecontractSum_MustPass_2.db";
            var firstSum = 800;
            var secondSum = 200;
            var db = new DbChildren(dbName);
            db.DeleteDb(dbName);
            //Act
            var child = new Child { ChildLastName = "second", ChildName = "First" };
            child.Precontracts.Add(new Precontract { Psum = firstSum });

            db.AddChildren([child]);
            //Assert
           

            //Act update
            var precontract = db.GetPrecontracts(pr => pr.PchildId == 1).Single();
            precontract.Pcomment = "0";
            

            var ret_code = db.UpdatePrecontracts([precontract]);

            Console.WriteLine(ret_code);
            //Aassert*/
        }
    }
}
