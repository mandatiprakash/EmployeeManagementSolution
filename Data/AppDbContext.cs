
using EmployeeManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Department>().HasMany(d => d.Employees).WithOne(e => e.Department).HasForeignKey(e => e.DepartmentId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
