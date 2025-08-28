using JobTracker.Data;
using JobTracker.Interfaces;
using JobTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Services
{
    public class UserService: IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JobTrackerContext _context;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JobTrackerContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public async Task<EmployeeDTO> Login([FromBody] LoginDTO loginInfo)
        {
            var user = await _userManager.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.UserName == loginInfo.UserName);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginInfo.Password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid username or password.");

            var employeeDto = new EmployeeDTO
            {
                Id = user.Employee.Id,
                Name = user.Employee.Name,
                Title = user.Employee.Title,
                Jobs = user.Employee.Jobs?.Select(j => new EmployeeJobDTO
                {
                    // Map job properties as needed
                }).ToList()
            };

            return employeeDto;
        }

        public async Task<EmployeeDTO> Register(RegisterDTO registerInfo)
        {

            var employee = new Employee
            {
                Name = registerInfo.Name,
                Title = registerInfo.Title,
                PayRate = registerInfo.PayRate
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var user = new ApplicationUser
            {
                UserName = registerInfo.UserName,
                EmployeeId = (int)employee.Id,
                Employee = employee
            };

            var result = await _userManager.CreateAsync(user, registerInfo.Password);
            if (!result.Succeeded)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
            employee.User = user;
            await _context.SaveChangesAsync();

            return new EmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                Title = employee.Title,
                Jobs = new List<EmployeeJobDTO>()
            };
        }
    }
}
