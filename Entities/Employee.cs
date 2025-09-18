
using System;
using System.Collections.Generic;

namespace EmployeeManagement.Api.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public decimal Salary { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        // Navigation
        public ICollection<Attendance>? Attendance { get; set; }
    }
}
