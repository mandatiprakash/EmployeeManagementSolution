
using System;

namespace EmployeeManagement.Api.DTOs.Employee
{
    public class EmployeeDto
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
        public string DepartmentName { get; set; }
    }
}
