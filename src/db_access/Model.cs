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
        public DbSet<YearMonthActivity> YearMonthActivities { get; set; }
        public DbSet<YearSub> YearSubs { get; set; }
        public DbSet<ContractFee> ContractFees { get; set; }
        #endregion

        #region Staff tables
        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<StaffActivity> StaffActivities { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<YearMonthStaffActivity> YearMonthStaffActivities { get; set; }
        public DbSet<Prepayment> Prepayments { get; set; }
        public DbSet<FinalPayment> FinalPayments { get; set; }
        #endregion

        #region Other Actovities Tables
        public DbSet<OtherActivity> OtherActivities { get; set; }
        public DbSet<YearMonthOther> YearMonthOthers { get; set; }
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
    [PrimaryKey(nameof(ChildId))]
    public class Child
    {
        public int ChildId { get; set; }
        public string ChildName { get; set; }
        public string ChildLastName { get; set; }
        public AlmanDefinitions.ContractType ChildContract { get; set; }
        public int ChildGroup { get; set; }
        public int ChildState { get; set; }
        public int ChildStartYear { get; set; }
        public int ChildMonthStart { get; set; }

    }

    [PrimaryKey(nameof(ActivityId))]
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
        public string PComment { get; set; }
    }


    [PrimaryKey(nameof(YMChildID), nameof(YMActivityID), nameof(YMActivitySum), nameof(Month), nameof(Year))]
    public class YearMonthActivity
    {
        public int YMChildID { get; set; }
        public int YMActivityID { get; set; }
        public int YMActivitySum { get; set; }      
        public int Month { get; set; }
        public int Year { get; set; }
        public AlmanDefinitions.WayOfPaingForActivity YMWayOfPaying {  get; set; }
        public AlmanDefinitions.WasPaid YMWasPaid { get; set; }
    }

    [PrimaryKey(nameof(YChildID), nameof(YYear))]
    public class YearSub
    {
        public int YChildID { get; set; }
        public int YYear { get; set; }
        public int YJanuary { get; set; }
        public AlmanDefinitions.WayOfPaying YJanuaryPayment { get; set; }
        public int YFebruary { get; set; }
        public AlmanDefinitions.WayOfPaying YFebruaryPayment { get; set; }
        public int YMarch { get; set; }
        public AlmanDefinitions.WayOfPaying YMarchPayment { get; set; }
        public int YApril { get; set; }
        public AlmanDefinitions.WayOfPaying YAprilPayment { get; set; }
        public int YMay { get; set; }
        public AlmanDefinitions.WayOfPaying YMayPayment { get; set; }
        public int YJune { get; set; }
        public AlmanDefinitions.WayOfPaying YJunePayment { get; set; }
        public int YJuly { get; set; }
        public AlmanDefinitions.WayOfPaying YJulyPayment { get; set; }
        public int YAugust { get; set; }
        public AlmanDefinitions.WayOfPaying YAugustPayment { get; set; }
        public int YSeptember { get; set; }
        public AlmanDefinitions.WayOfPaying YSeptemberPayment { get; set; }
        public int YOctober { get; set; }
        public AlmanDefinitions.WayOfPaying YOctoberPayment { get; set; }
        public int YNovember { get; set; }
        public AlmanDefinitions.WayOfPaying YNovemberPayment { get; set; }
        public int YDecember { get; set; }
        public AlmanDefinitions.WayOfPaying YDecemberPayment { get; set; }
    }

    [PrimaryKey(nameof(CFChildId), nameof(CFMonth), nameof(CFYear))]
    public class ContractFee
    {
        public int CFChildId { get; set; }
        public int CFMonth { get; set; }
        public int CFYear { get; set; }
        public int CFSumPaid { get; set; }
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
        public AlmanDefinitions.StaffMemberState State { get; set; }
    }

    [PrimaryKey(nameof(PositionId))]
    public class Position
    {
        public int PositionId { get; set;}
        public string PositionName { get; set; }
        public int PositionSalary { get; set; }
    }

    [PrimaryKey(nameof(StaffActivityId))]
    public class StaffActivity
    {
        public int StaffActivityId { get; set; }
        public string ActivityName { get; set; }
    }


    [PrimaryKey(nameof(StaffMemberId), nameof(StaffActivityId), nameof(Month), nameof(Year))]
    public class YearMonthStaffActivity
    {
        public int StaffMemberId { get; set; }
        public int StaffActivityId { get; set; }
        public int SumPaid { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }


    [PrimaryKey(nameof(StaffMemberId), nameof(Year), nameof(Month))]
    public class Prepayment
    {
        public int StaffMemberId { get; set; }
        public int PaidSum { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public AlmanDefinitions.WasPaid WasPaid { get; set; }

    }

    [PrimaryKey(nameof(StaffMemberId), nameof(Year), nameof(Month))]
    public class FinalPayment
    {
        public int StaffMemberId { get; set; }
        public int PaidSum { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public AlmanDefinitions.WasPaid WasPaid { get; set; }
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

    [PrimaryKey(nameof(OtherActivityId), nameof(Month), nameof(Year))]
    public class YearMonthOther
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int FirstWeek { get; set; }
        public int SecondWeek { get; set; }
        public int ThirdWeek { get; set; }
        public int FourthWeek { get; set;}
        public int FifthWeek { get; set; }
        public int OtherActivityId { get; set; }
    }

    #endregion
}