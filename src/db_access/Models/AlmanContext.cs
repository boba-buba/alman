using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DbAccess.Models;

public partial class AlmanContext : DbContext
{
    public AlmanContext()
    {
    }

    public AlmanContext(DbContextOptions<AlmanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Child> Children { get; set; }

    public virtual DbSet<ContractFee> ContractFees { get; set; }

    public virtual DbSet<FinalPayment> FinalPayments { get; set; }

    public virtual DbSet<OtherActivity> OtherActivities { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Precontract> Precontracts { get; set; }

    public virtual DbSet<Prepayment> Prepayments { get; set; }

    public virtual DbSet<StaffActivity> StaffActivities { get; set; }

    public virtual DbSet<StaffMember> StaffMembers { get; set; }

    public virtual DbSet<YearMonthActivity> YearMonthActivities { get; set; }

    public virtual DbSet<YearMonthOther> YearMonthOthers { get; set; }

    public virtual DbSet<YearMonthStaffActivity> YearMonthStaffActivities { get; set; }

    public virtual DbSet<YearSub> YearSubs { get; set; }
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = new SqliteConnectionStringBuilder();

        connectionString.DataSource = DbPath;
        connectionString.ForeignKeys = true;
        connectionString.Pooling = true;

        string builtString = connectionString.ToString();
        optionsBuilder.UseSqlite(builtString).LogTo(Console.WriteLine, LogLevel.Information).EnableThreadSafetyChecks().EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Child>(entity =>
        {
            entity.Property(e => e.ChildContract).HasColumnType("INT");
            entity.Property(e => e.ChildLastName).HasColumnName("ChildLastNAme");
            entity.Property(e => e.ChildName).HasColumnType("TEXT");
            entity.Property(e => e.ChildState).HasDefaultValue(1);
        });

        modelBuilder.Entity<ContractFee>(entity =>
        {
            entity.HasKey(e => new { e.CfchildId, e.Cfmonth, e.Cfyear });

            entity.Property(e => e.CfchildId).HasColumnName("CFChildId");
            entity.Property(e => e.Cfmonth).HasColumnName("CFMonth");
            entity.Property(e => e.Cfyear).HasColumnName("CFYear");
            entity.Property(e => e.CfsumPaid).HasColumnName("CFSumPaid");

            entity.HasOne(d => d.Cfchild).WithMany(p => p.ContractFees)
                .HasForeignKey(d => d.CfchildId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FinalPayment>(entity =>
        {
            entity.HasKey(e => new { e.StaffMemberId, e.Month, e.Year });

            entity.HasOne(d => d.StaffMember).WithMany(p => p.FinalPayments)
                .HasForeignKey(d => d.StaffMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<OtherActivity>(entity =>
        {
            entity.HasKey(e => e.OtherId);

            entity.Property(e => e.OtherId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId);

            //entity.Property(e => e.PositionId).ValueGeneratedNever();
            //entity.Property(e => e.PositionName).HasColumnType("TEXT").HasColumnName("PositionName");
            //entity.Property(e => e.PositionSalary).HasColumnType("INT").HasColumnName("PositionName");

        });

        modelBuilder.Entity<Precontract>(entity =>
        {
            entity.HasKey(e => new { e.PchildId });

            entity.Property(e => e.PchildId).HasColumnName("PChildID");
            entity.Property(e => e.Psum).HasColumnName("PSum");
            entity.Property(e => e.Pcomment).HasColumnName("PComment");
            entity.Property(e => e.PYear).HasColumnName("PYear");
            entity.Property(e => e.PMonth).HasColumnName("PMonth");

            entity.HasOne(d => d.Pchild).WithMany(p => p.Precontracts)
                .HasForeignKey(d => d.PchildId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Prepayment>(entity =>
        {
            entity.HasKey(e => new { e.StaffMemberId, e.Year, e.Month });

            entity.HasOne(d => d.StaffMember).WithMany(p => p.Prepayments)
                .HasForeignKey(d => d.StaffMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<StaffActivity>(entity =>
        {
            entity.Property(e => e.StaffActivityId).ValueGeneratedNever();
        });

        modelBuilder.Entity<StaffMember>(entity =>
        {
            entity.Property(e => e.StaffMemberId).ValueGeneratedNever();

            entity.HasOne(d => d.Position).WithMany(p => p.StaffMembers).HasForeignKey(d => d.PositionId);
        });

        modelBuilder.Entity<YearMonthActivity>(entity =>
        {
            entity.HasKey(e => new { e.YmchildId, e.YmactivityId, e.Month, e.Year });

            entity.Property(e => e.YmchildId).HasColumnName("YMChildId");
            entity.Property(e => e.YmactivityId).HasColumnName("YMActivityId");
            entity.Property(e => e.YmactivitySum).HasColumnName("YMActivitySum");
            entity.Property(e => e.YmwasPaid).HasColumnName("YMWasPaid");
            entity.Property(e => e.YmwayOfPaying).HasColumnName("YMWayOfPaying");

            entity.HasOne(d => d.Ymactivity).WithMany(p => p.YearMonthActivities)
                .HasForeignKey(d => d.YmactivityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Ymchild).WithMany(p => p.YearMonthActivities)
                .HasForeignKey(d => d.YmchildId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<YearMonthOther>(entity =>
        {
            entity.HasKey(e => new { e.OtherActivityId, e.Month, e.Year });

            entity.HasOne(d => d.OtherActivity).WithMany(p => p.YearMonthOthers)
                .HasForeignKey(d => d.OtherActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<YearMonthStaffActivity>(entity =>
        {
            entity.HasKey(e => new { e.StaffMemberId, e.StaffActivityId, e.Month, e.Year });

            entity.Property(e => e.SumPaid).HasColumnType("NUMERIC");

            entity.HasOne(d => d.StaffActivity).WithMany(p => p.YearMonthStaffActivities)
                .HasForeignKey(d => d.StaffActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StaffMember).WithMany(p => p.YearMonthStaffActivities)
                .HasForeignKey(d => d.StaffMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<YearSub>(entity =>
        {
            entity.HasKey(e => new { e.YchildId, e.Yyear });

            entity.Property(e => e.YchildId).HasColumnName("YChildID");
            entity.Property(e => e.Yyear)
                .HasColumnType("INT")
                .HasColumnName("YYear");
            entity.Property(e => e.Yapril).HasColumnName("YApril");
            entity.Property(e => e.YaprilPayment).HasColumnName("YAprilPayment");
            entity.Property(e => e.Yaugust).HasColumnName("YAugust");
            entity.Property(e => e.YaugustPayment).HasColumnName("YAugustPayment");
            entity.Property(e => e.Ydecember).HasColumnName("YDecember");
            entity.Property(e => e.YdecemberPayment).HasColumnName("YDecemberPayment");
            entity.Property(e => e.Yfebruary).HasColumnName("YFebruary");
            entity.Property(e => e.YfebruaryPayment).HasColumnName("YFebruaryPayment");
            entity.Property(e => e.Yjanuary).HasColumnName("YJanuary");
            entity.Property(e => e.YjanuaryPayment).HasColumnName("YJanuaryPayment");
            entity.Property(e => e.Yjuly).HasColumnName("YJuly");
            entity.Property(e => e.YjulyPayment).HasColumnName("YJulyPayment");
            entity.Property(e => e.Yjune).HasColumnName("YJune");
            entity.Property(e => e.YjunePayment).HasColumnName("YJunePayment");
            entity.Property(e => e.Ymarch).HasColumnName("YMarch");
            entity.Property(e => e.YmarchPayment).HasColumnName("YMarchPayment");
            entity.Property(e => e.Ymay).HasColumnName("YMay");
            entity.Property(e => e.YmayPayment).HasColumnName("YMayPayment");
            entity.Property(e => e.Ynovember).HasColumnName("YNovember");
            entity.Property(e => e.YnovemberPayment).HasColumnName("YNovemberPayment");
            entity.Property(e => e.Yoctober).HasColumnName("YOctober");
            entity.Property(e => e.YoctoberPayment).HasColumnName("YOctoberPayment");
            entity.Property(e => e.Yseptember).HasColumnName("YSeptember");
            entity.Property(e => e.YseptemberPayment).HasColumnName("YSeptemberPayment");

            entity.HasOne(d => d.Ychild).WithMany(p => p.YearSubs)
                .HasForeignKey(d => d.YchildId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
