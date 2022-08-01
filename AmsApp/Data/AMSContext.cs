using System;
using System.Collections.Generic;
//using AmsApp.Dto;
using AmsApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AmsApp.Data
{
    public partial class AMSContext : DbContext
    {
        public AMSContext()
        {
        }

        public AMSContext(DbContextOptions<AMSContext> options)
            : base(options)
        {
        }

        public async Task<T> ExecuteScalarAsync<T>(string rawSql, params object[] parameters)
        {
            var conn = Database.GetDbConnection();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = rawSql;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                await conn.OpenAsync();
                return (T)await command.ExecuteScalarAsync();
            }
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<VwEmployees> VwEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.AdminPassword)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('2c16d10036a7736c42aa7adea96719c5')");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsLoggedin).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastLoggedIn).HasColumnType("datetime");

                entity.Property(e => e.LoggedInIp)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password2ModifiedOn).HasColumnType("date");

                entity.Property(e => e.PasswordModifiedOn).HasColumnType("date");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<VwEmployees>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwEmployees");

                entity.Property(e => e.BioMetricId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BranchName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DesignationTitle)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Doj)
                    .HasColumnType("date")
                    .HasColumnName("DOJ");

                entity.Property(e => e.EmployeeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FromTime)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FromTime");

                entity.Property(e => e.Mobile1)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoFile)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SubDepartmentName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ToTime)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ToTime");

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.Property(e => e.WhatsApp)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
