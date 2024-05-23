using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CanteenManage.Models;

public partial class CanteenManageContext : DbContext
{
    public CanteenManageContext()
    {
    }

    public CanteenManageContext(DbContextOptions<CanteenManageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAttendance> CustomerAttendances { get; set; }

    public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<VendPurchase> VendPurchases { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-SM1FNIC\\SQLEXPRESS;Database=CANTEEN_MANAGE;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69261C97E68B2D");

            entity.ToTable("Attendance");

            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_Employee");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB85C69EA982");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PrepaidBalance)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("prepaid_balance");
            entity.Property(e => e.SubscriptionExpiryDate).HasColumnName("subscription_expiry_date");
            entity.Property(e => e.SubscriptionStartDate).HasColumnName("subscription_start_date");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("type");
        });

        modelBuilder.Entity<CustomerAttendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Customer__8B69261C208918B8");

            entity.ToTable("CustomerAttendance");

            entity.Property(e => e.AttendanceId).ValueGeneratedNever();
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAttendances)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__CustomerA__Custo__625A9A57");
        });

        modelBuilder.Entity<CustomerOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Customer__46596229D8E0DEE1");

            entity.ToTable("Customer_Orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RemainingBalance)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("remaining_balance");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Customer___custo__5BE2A6F2");

            entity.HasOne(d => d.Product).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Customer___produ__5CD6CB2B");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C134C9C1A7A4D521");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("employeeId");
            entity.Property(e => e.Aadhar)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("aadhar");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.JoiningDate).HasColumnName("joiningDate");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.ResignDate).HasColumnName("resignDate");
            entity.Property(e => e.Salary)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("salary");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Menu__47027DF552D856B4");

            entity.ToTable("Menu");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.SalaryId).HasName("PK__Salary__33631F98A5BE3C22");

            entity.ToTable("Salary");

            entity.Property(e => e.SalaryId).HasColumnName("salaryId");
            entity.Property(e => e.EmployeeId).HasColumnName("employeeId");
            entity.Property(e => e.PaidAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("paidAmount");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("paymentDate");
            entity.Property(e => e.SalaryMonth)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("salaryMonth");

            entity.HasOne(d => d.Employee).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Salary__employee__4CA06362");
        });

        modelBuilder.Entity<VendPurchase>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__vend_pur__465962297A235987");

            entity.ToTable("vend_purchase");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("product_name");
            entity.Property(e => e.VendorId).HasColumnName("vendor_id");

            entity.HasOne(d => d.Vendor).WithMany(p => p.VendPurchases)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("FK__vend_purc__vendo__29221CFB");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.VendorId).HasName("PK__vendors__0F7D2B7891B94F23");

            entity.ToTable("vendors");

            entity.Property(e => e.VendorId).HasColumnName("vendor_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.VendorName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("vendor_name");
            entity.Property(e => e.VendorType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("vendor_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
