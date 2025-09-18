
using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Api.DTOs.Employee
{
    public class EmployeeCreateDto
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public decimal Salary { get; set; }
        public int? DepartmentId { get; set; }
    }
}
