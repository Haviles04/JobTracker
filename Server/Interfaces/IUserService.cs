using JobTracker.Models;

namespace JobTracker.Interfaces
{
    public interface IUserService
    {
        public Task<EmployeeDTO> Login(LoginDTO login);
        public Task<EmployeeDTO> Register(RegisterDTO register);
    }
}
