using JobTracker.Models;

namespace JobTracker.Interfaces
{
    public interface IEmployeeService
    {
        bool EmployeeExists(long id);
        Task<List<EmployeeDTO>?> GetAllEmployees();
        Task<EmployeeDTO> GetEmployee(long id);
        Task UpdateEmployee(long id, Employee employee);
        Task<EmployeeDTO> CreateEmployee(Employee employee);
    }
}
