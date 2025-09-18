
using System;

namespace EmployeeManagement.Api.Entities
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public DateTime Date { get; set; }
        public bool Present { get; set; }
    }
}
