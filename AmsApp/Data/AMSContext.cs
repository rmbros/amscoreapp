using System;
using System.Collections.Generic;
using AmsApp.Dto;
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
        public virtual DbSet<OBDisposition> OBDispositions { get; set; } = null!;
        public virtual DbSet<OBCallHistory> OBCallHistories { get; set; } = null!;
        public virtual DbSet<OBLead> OBLeads { get; set; } = null!;
        public virtual DbSet<OBAssignedLead> OBAssignedLeads { get; set; } = null!;
        public virtual DbSet<LeadCountDto> LeadCountDtos { get; set; } = null!;
        public virtual DbSet<AppAppointment> AppAppointments { get; set; } = null!;
        public virtual DbSet<VwVisit> VwVisits { get; set; } = null!;
        public virtual DbSet<CCTeam> CCTeams { get; set; } = null!;
        public virtual DbSet<OBAgentDaySummaryDto> OBAgentDaySummary { get; set; } = null!;

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

            modelBuilder.Entity<OBDisposition>(entity =>
            {
                entity.ToTable("OBDispositions");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Status).HasDefaultValueSql("(N'1')");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OBCallHistory>(entity =>
            {
                entity.ToTable("OBCallHistory");

                entity.Property(e => e.CallDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disposition).HasDefaultValueSql("((0))");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.NextCallDate).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<OBLead>(entity =>
            {
                entity.ToTable("OBLeads");

                entity.HasIndex(e => e.Mobile, "OBLeads$Mobile")
                    .IsUnique();

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.AllocatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.AltMobile).HasMaxLength(20);

                entity.Property(e => e.AppointmentDate).HasColumnType("datetime");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disposition).HasDefaultValueSql("((0))");

                entity.Property(e => e.Dist).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.LastCallOn).HasColumnType("datetime");

                entity.Property(e => e.LastCalledBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NextCallDate).HasColumnType("datetime");

                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.Property(e => e.OBLeadUploadHistoryId).HasColumnName("OBLeadUploadHistoryId");

                entity.Property(e => e.PatientId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State).HasMaxLength(100);

                entity.Property(e => e.Status).HasDefaultValueSql("(N'1')");
            });

            modelBuilder.Entity<AppAppointment>(entity =>
            {
                entity.HasIndex(e => e.Mobile, "AppAppointments$Mobile")
                    .IsUnique();

                entity.Property(e => e.AppointmentDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Location).HasMaxLength(1000);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PatientId).HasMaxLength(20);

                entity.Property(e => e.Status).HasDefaultValueSql("(N'1')");
            });

            modelBuilder.Entity<VwVisit>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Visits");

                entity.Property(e => e.AppType)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.AppointmentDate).HasColumnType("datetime");

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PatientId).HasMaxLength(50);

            });

            modelBuilder.Entity<CCTeam>(entity =>
            {
                entity.ToTable("CCTeams");

                entity.Property(e => e.CCE).HasColumnName("CCE");

                entity.Property(e => e.CCTL).HasColumnName("CCTL");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(N'1')");
            });

            modelBuilder.Entity<OBAssignedLead>(entity =>
            {
                entity.ToTable("OBAssignedLeads");

                entity.HasIndex(e => e.Mobile, "OBAssignedLeads$Mobile")
                    .IsUnique();

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.AllocatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.AppointmentDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disposition).HasDefaultValueSql("((0))");

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NextCallDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasDefaultValueSql("(N'1')");
            });

            modelBuilder.Entity<LeadCountDto>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<OBAgentDaySummaryDto>(entity =>
            {
                entity.HasNoKey();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
