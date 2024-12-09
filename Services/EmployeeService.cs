using JobTracker.Models;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Services
{
    public class EmployeeService(JobTrackerContext context)
    {
        private readonly JobTrackerContext _context = context;

        private bool EmployeeExists(long id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        public async Task<List<EmployeeDTO>?> GetAllEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return employees?.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.Name,
                Title = e.Title,
            }).ToList();
        }

        public async Task<EmployeeDTO> GetEmployee(long id)
        {

            var employee = await _context.Employees.Include(e => e.Jobs).FirstOrDefaultAsync(j => j.Id == id);

            if (employee == null)
            {
                throw new ArgumentException($"Employee with Id {id} doesn't exist");
            }

            return new EmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                Title = employee.Title,
                Jobs = employee?.Jobs?.Select(j => new EmployeeJobDTO
                {
                    Id = j.Id,
                    JobNumber = j.JobNumber,
                    Location = j.Location,
                }).ToList()
            };
        }

        public async Task UpdateEmployee(long id, Employee employee)
        {
            if (id != employee.Id)
            {
                throw new ArgumentException("Id does not match Employee Id");
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    throw new ArgumentException("Employee does not exist");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<EmployeeDTO> CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return new EmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                Title = employee.Title,
            };
        }

    }
}
