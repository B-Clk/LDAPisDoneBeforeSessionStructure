using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Yeni_Web_Projem.Models;

public partial class EmailDatabaseContext : DbContext
{
    public EmailDatabaseContext()
    {
    }

    public EmailDatabaseContext(DbContextOptions<EmailDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Email> Emails { get; set; }

    public virtual DbSet<EmailType> EmailTypes { get; set; }

    public virtual DbSet<PhoneHistory> PhoneHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserChangesEmail> UserChangesEmails { get; set; }

    public virtual DbSet<UserResponsibleEmail> UserResponsibleEmails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-KRDDPMR\\SQLEXPRESS;Database=EmailDatabase;Trusted_Connection=True;Encrypt=False;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__F9B8344D1B880498");

            entity.Property(e => e.DepartmentId)
                .ValueGeneratedNever()
                .HasColumnName("departmentID");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("departmentName");
        });

        modelBuilder.Entity<Email>(entity =>
        {
            entity.HasKey(e => e.EmailId).HasName("PK__Emails__87355E529511C6C4");

            entity.Property(e => e.EmailId)
                .ValueGeneratedNever()
                .HasColumnName("emailID");
            entity.Property(e => e.EmailCreationDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("emailCreationDate");
            entity.Property(e => e.EmailDescription)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("emailDescription");
            entity.Property(e => e.EmailName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("emailName");
            entity.Property(e => e.EmailTypeId).HasColumnName("emailTypeID");

            entity.HasOne(d => d.EmailType).WithMany(p => p.Emails)
                .HasForeignKey(d => d.EmailTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Emails__emailTyp__5070F446");
        });

        modelBuilder.Entity<EmailType>(entity =>
        {
            entity.HasKey(e => e.EmailTypeId).HasName("PK__EmailTyp__D3178820D78E4BF6");

            entity.Property(e => e.EmailTypeId)
                .ValueGeneratedNever()
                .HasColumnName("emailTypeID");
            entity.Property(e => e.EmailTypeName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("emailTypeName");
        });

        modelBuilder.Entity<PhoneHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PhoneHistory");

            entity.Property(e => e.ChangeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("changeDate");
            entity.Property(e => e.NewPhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("newPhoneNumber");
            entity.Property(e => e.OldPhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("oldPhoneNumber");
            entity.Property(e => e.PhoneChangeId).HasColumnName("phoneChangeId");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PhoneHist__userI__5629CD9C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__CB9A1CDF5C2D4F9F");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userID");
            entity.Property(e => e.DepartmentId).HasColumnName("departmentID");
            entity.Property(e => e.FirstLoginDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("firstLoginDate");
            entity.Property(e => e.FullName)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("fullName");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.Department).WithMany(p => p.Users)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Users__departmen__4D94879B");
        });

        modelBuilder.Entity<UserChangesEmail>(entity =>
        {
            entity.HasKey(e => e.ChangeId).HasName("PK__UserChan__1A350F78D6533426");

            entity.Property(e => e.ChangeId)
                .ValueGeneratedNever()
                .HasColumnName("changeID");
            entity.Property(e => e.ChangeDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("changeDate");
            entity.Property(e => e.NextEmailId).HasColumnName("nextEmailID");
            entity.Property(e => e.NextEmailName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nextEmailName");
            entity.Property(e => e.PreviousEmailId).HasColumnName("previousEmailID");
            entity.Property(e => e.PreviousEmailName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("previousEmailName");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.UserChangesEmails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserChang__userI__6FE99F9F");
        });

        modelBuilder.Entity<UserResponsibleEmail>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.EmailId }).HasName("pk_UserEmail");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.EmailId).HasColumnName("emailID");
            entity.Property(e => e.PermissionEndDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("permissionEndDate");
            entity.Property(e => e.PermissionStartDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("permissionStartDate");

            entity.HasOne(d => d.Email).WithMany(p => p.UserResponsibleEmails)
                .HasForeignKey(d => d.EmailId)
                .HasConstraintName("FK__UserRespo__email__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.UserResponsibleEmails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRespo__userI__534D60F1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
