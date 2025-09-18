
using EmployeeManagement.Api.Entities;
using System.Linq;

namespace EmployeeManagement.Api.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Users.Any()) return;

            var admin = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin"
            };
            db.Users.Add(admin);

            if (!db.Departments.Any())
            {
                db.Departments.AddRange(
                    new Department { Name = "HR" },
                    new Department { Name = "Engineering" },
                    new Department { Name = "Sales" }
                );
            }

            if (!db.Employees.Any())
            {
                db.Employees.AddRange(
                    new Employee { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DateOfJoining = System.DateTime.UtcNow.AddYears(-1), Salary = 50000 },
                    new Employee { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", DateOfJoining = System.DateTime.UtcNow.AddYears(-2), Salary = 60000 }
                );
            }

            db.SaveChanges();
        }
    }
}
