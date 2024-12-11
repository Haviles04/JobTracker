using Microsoft.AspNetCore.Mvc;
using JobTracker.Models;
using JobTracker.Services;

namespace JobTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(EmployeeService employeeService) : ControllerBase
    {
        private readonly EmployeeService _employeeService = employeeService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployees();
                return Ok(employees);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(long id)
        {
            try
            {
                var employee = await _employeeService.GetEmployee(id);
                return Ok(employee);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error {e.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(long id, Employee employee)
        {
            try
            {
                await _employeeService.UpdateEmployee(id, employee);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Interal Server Error {e.Message}");
            }
                
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            var newEmployee = await _employeeService.CreateEmployee(employee);

            return CreatedAtAction("GetEmployee", new { id = newEmployee.Id }, newEmployee);
        }

        //// DELETE: api/Employees/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEmployee(long id)
        //{
        //    var employee = await _context.Employees.FindAsync(id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Employees.Remove(employee);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
