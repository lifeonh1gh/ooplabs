using Microsoft.EntityFrameworkCore;
using Reports.Entities;

namespace Reports.Helpers
{
    public sealed class ReportsContext : DbContext
    {
        public ReportsContext(DbContextOptions<ReportsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<WeeklyReport> WeeklyReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<TaskModel>().ToTable("Tasks");
            modelBuilder.Entity<TaskModel>().HasOne(model => model.AssignedEmployee);
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Comment>().HasOne(model => model.SenderEmployee);
            modelBuilder.Entity<Comment>().HasOne(model => model.Task);
            modelBuilder.Entity<Report>().ToTable("Reports");
            modelBuilder.Entity<Report>().HasOne(model => model.AssignedEmployee);
            modelBuilder.Entity<WeeklyReport>().ToTable("WeeklyReports");
            modelBuilder.Entity<WeeklyReport>().HasOne(model => model.Report);
            modelBuilder.Entity<WeeklyReport>().HasOne(model => model.Task);
            base.OnModelCreating(modelBuilder);
        }
    }
}