
using ClosedXML.Excel;
using EmployeeManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public ReportsController(IEmployeeService employeeService) { _employeeService = employeeService; }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var employees = (await _employeeService.GetAllAsync()).ToList();
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Employees");
            ws.Cell(1,1).Value = "Id";
            ws.Cell(1,2).Value = "Name";
            ws.Cell(1,3).Value = "Email";
            ws.Cell(1,4).Value = "Salary";
            for(int i=0;i<employees.Count;i++)
            {
                var r = i+2;
                ws.Cell(r,1).Value = employees[i].Id;
                ws.Cell(r,2).Value = employees[i].FirstName + " " + employees[i].LastName;
                ws.Cell(r,3).Value = employees[i].Email;
                ws.Cell(r,4).Value = employees[i].Salary;
            }
            using var ms = new System.IO.MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "employees.xlsx");
        }
    }
}
