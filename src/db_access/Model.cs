using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess
{

    public class AlmanContext : DbContext
    {
        public DbSet<Child> Children { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Precontract> Precontracts { get; set; }
        public DbSet<YearFee> YearFees { get; set; }

        public string DbPath { get; }

        public AlmanContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "alman.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");


    }

    public class Child
    {
        public int ChildId { get; set; }
        public string ChildName { get; set; }
        public string ChildLastName { get; set; }
        public string ChildContract { get; set; }
        public int ChildGroup { get; set; }
    }

    public class Activity
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int ActivityPrice { get; set; }
    }

    [PrimaryKey(nameof(PChildID), nameof(PSum))]
    public class Precontract
    {
        public int PChildID { get; set; }
        public int PSum { get; set; }
    }

    [PrimaryKey(nameof(YFChildID), nameof(YFDate), nameof(YFSum))]
    public class YearFee
    {
        public int YFChildID { get; set; }
        public DateTime YFDate { get; set; }
        public int YFSum { get; set; }

    }
}