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
        #region Children tables
        public DbSet<Child> Children { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Precontract> Precontracts { get; set; }
        public DbSet<YearFee> YearFees { get; set; }
        public DbSet<YearMonthActivity> YearMonthActivities { get; set; }
        public DbSet<YearSub> yearSubs { get; set; }
        #endregion

        #region Staff tables
        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<StaffActivity> StaffActivities { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<MonthYearStaffActivity> YearMonthStaffActivities { get; set; }
        public DbSet<Prepayment> Prepayments { get; set; }

        #endregion

        #region Other Actovities Tables
        public DbSet<OtherActivity> OtherActivities { get; set; }
        public DbSet<YearMonthOther> yearMonthOthers { get; set; }
        #endregion


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

    #region Children Entities
    public class Child
    {
        public int ChildId { get; set; }
        public string ChildName { get; set; }
        public string ChildLastName { get; set; }
        public string ChildContract { get; set; }
        public int ChildGroup { get; set; }
        public string ChildState { get; set; }
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

    [PrimaryKey(nameof(YMChildID), nameof(YMActivityID), nameof(YMActivitySum), nameof(Month), nameof(Year))]
    public class YearMonthActivity
    {
        public int YMChildID { get; set; }
        public int YMActivityID { get; set; }
        public int YMActivitySum { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

    }

    [PrimaryKey(nameof(YYChildID), nameof(YYear), nameof(YSum), nameof(YDate))]
    public class YearSub
    {
        public int YYChildID { get; set; }
        public int YYear { get; set; }
        public int YSum { get; set; }
        public DateOnly YDate { get; set; }
    }
    #endregion

    #region Staff Entities

    [PrimaryKey(nameof(StaffMemberId))]
    public class StaffMember
    {
        public int StaffMemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public int PositionId { get; set; }
    }

    [PrimaryKey(nameof(PositionId))]
    public class Position
    {
        public int PositionId { get; set;}
        public string PositionNAme { get; set; }
        public int PositionSalary { get; set; }
    }

    [PrimaryKey(nameof(StaffActivityId))]
    public class StaffActivity
    {
        public int StaffActivityId { get; set; }
        public string ActivityName { get; set; }
    }


    [PrimaryKey(nameof(StaffMemberId), nameof(StaffActivityId), nameof(Month), nameof(Year))]
    public class MonthYearStaffActivity
    {
        public int StaffMemberId { get; set; }
        public int StaffActivityId { get; set; }
        public int Earned { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }


    [PrimaryKey(nameof(StaffMemberId), nameof(PayedSum), nameof(PaymentDay))]
    public class Prepayment
    {
        public int StaffMemberId { get; set; }
        public int PayedSum { get; set; }
        public int Month { get; set; }
        public DateOnly PaymentDay { get; set; }
    }

    //Plat na konci? Teoreticky lze vypocitat z Position salary + sum(activities) - prepayment sum
    #endregion

    #region Other Entities
    [PrimaryKey(nameof(OtherId))]
    public class OtherActivity
    {
        public int OtherId { get; set; }
        public int OtherName { get; set; }
    }

    public class YearMonthOther
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int FirstWeek { get; set; }
        public int SecondWeek { get; set; }
        public int ThirdWeek { get; set; }
        public int FourthWeek { get; set;}

        //Fifth week?
        public int OtherActivityId { get; set; }
    }

    #endregion
}