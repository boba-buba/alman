using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;

namespace DbAccess.Models;

public partial class AlmanContext : DbContext
{
    /// <summary>
    /// default ctor
    /// </summary>
    public AlmanContext()
    {
    }

    public AlmanContext(DbContextOptions<AlmanContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Activities table.
    /// </summary>
    public virtual DbSet<Activity> Activities { get; set; }

    /// <summary>
    /// Children table.
    /// </summary>
    public virtual DbSet<Child> Children { get; set; }

    /// <summary>
    /// Contract fees table.
    /// </summary>
    public virtual DbSet<ContractFee> ContractFees { get; set; }

    /// <summary>
    /// Final Payments table.
    /// </summary>
    public virtual DbSet<FinalPayment> FinalPayments { get; set; }

    /// <summary>
    /// Precontracts table.
    /// </summary>
    public virtual DbSet<Precontract> Precontracts { get; set; }

    /// <summary>
    /// Staff activities table.
    /// </summary>
    public virtual DbSet<StaffActivity> StaffActivities { get; set; }

    /// <summary>
    /// Staff members table.
    /// </summary>
    public virtual DbSet<StaffMember> StaffMembers { get; set; }

    /// <summary>
    /// Monthly children activities table.
    /// </summary>
    public virtual DbSet<YearMonthActivity> YearMonthActivities { get; set; }

    /// <summary>
    /// Monthly other activities that are not staff or child activities.
    /// </summary>
    public virtual DbSet<YearMonthOther> YearMonthOthers { get; set; }

    /// <summary>
    /// Monthly staff activities table.
    /// </summary>
    public virtual DbSet<YearMonthStaffActivity> YearMonthStaffActivities { get; set; }

    /// <summary>
    /// Yearly children subscriptions.
    /// </summary>
    public virtual DbSet<YearSub> YearSubs { get; set; }

    /// <summary>
    /// Monthly Expenses table.
    /// </summary>
    public virtual DbSet<Expense> YearMonthExpenses { get; set; }

    /// <summary>
    /// Users that can access database table.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// Year results (sums of income and expenses) table.
    /// </summary>
    public virtual DbSet<YearResult> YearResults { get; set; }

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
            entity.HasKey(entity => entity.Id);

            entity.Property(e => e.ChildContract).HasColumnType("INT");
            entity.Property(e => e.ChildLastName).HasColumnName("ChildLastNAme");
            entity.Property(e => e.ChildName).HasColumnType("TEXT");
            entity.Property(e => e.ChildState).HasDefaultValue(1);
        });

        modelBuilder.Entity<ContractFee>(entity =>
        {
            entity.HasKey(e => e.Id);

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
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.StaffMember).WithMany(p => p.FinalPayments)
                .HasForeignKey(d => d.StaffMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Precontract>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.PchildId).HasColumnName("PChildID");
            entity.Property(e => e.Psum).HasColumnName("PSum");
            entity.Property(e => e.Pcomment).HasColumnName("PComment");
            entity.Property(e => e.PYear).HasColumnName("PYear");
            entity.Property(e => e.PMonth).HasColumnName("PMonth");

            entity.HasOne(d => d.Pchild).WithMany(p => p.Precontracts)
                .HasForeignKey(d => d.PchildId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StaffActivity>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<StaffMember>(entity =>
        {
            entity.HasKey(e => e.Id);

        });

        modelBuilder.Entity<YearMonthActivity>(entity =>
        {
            entity.HasKey(e => e.Id);

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
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<YearMonthStaffActivity>(entity =>
        {
            entity.HasKey(e =>  e.Id);

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
            entity.HasKey(e => e.Id);

            entity.Property(e => e.YchildId).HasColumnName("YChildID");

            entity.HasOne(d => d.Ychild).WithMany(p => p.YearSubs)
                .HasForeignKey(d => d.YchildId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Expense>(entity => 
        {
            entity.HasKey(e => new { e.Id });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e =>  e.Id);
        });

        modelBuilder.Entity<YearResult>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    /// <summary>
    /// Returns instance of one of the declared DbSets, that represents the table.
    /// </summary>
    /// <typeparam name="TEntity"> Type of the entity that reperesents the row of the table. </typeparam>
    /// <returns> Instance of the DbSet </returns>
    /// <exception cref="InvalidOperationException"> If trying to find dbSet for the TEntity that was not defined in DbContext. </exception>
    /// <exception cref="ArgumentNullException"> If found dbSet is null </exception>
    public DbSet<TEntity> GetDeclaredDbSet<TEntity>() where TEntity : class
    {
        Type contextType = this.GetType();

        var dbSetProperties = contextType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
        var dbSetProperty = dbSetProperties.FirstOrDefault(p => p.PropertyType == typeof(DbSet<TEntity>));

        if (dbSetProperty is null)
        {
            throw new InvalidOperationException($"No dbSet found for entiy type {typeof(TEntity).Name}.");
        }
        var dbSet = dbSetProperty.GetValue(this);
        if (dbSet is null)
        {
            throw new ArgumentNullException($"No dbSet defined for entiy type {typeof(TEntity).Name}.");
        }
        return (DbSet<TEntity>)dbSet;
    }
}
