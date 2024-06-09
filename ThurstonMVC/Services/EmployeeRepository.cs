using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThurstonMVC.Contracts;
using ThurstonMVC.Data;
using ThurstonMVC.Models.Entities;
using ThurstonMVC.Models.ViewModel;

namespace ThurstonMVC.Services
{
    public class EmployeeRepository : IEmployee
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _context.Employees.ToListAsync();
        }
        public async Task Create(AddEmployeeViewModel employeeCreateModel)
        {
            var emp = new Employee()
            {
                FirstName = employeeCreateModel.FirstName,
                LastName = employeeCreateModel.LastName,
                Email = employeeCreateModel.Email,
                DateOfBirth = employeeCreateModel.DateOfBirth,
            };

            await _context.Employees.AddAsync(emp);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee> GetEmployeeById(long employeeId)
        {
            return await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == employeeId) ?? null;
        }
        public async Task<bool> GetEmployeeByEmail(string employeeEmail)
        {
            return await _context.Employees.AnyAsync(emp => emp.Email == employeeEmail);
        }

        public async Task Update(EditEmployeeViewModel editEmployee)
        {
            var getEmployeeObject = await GetEmployeeById(editEmployee.Id);

            getEmployeeObject.FirstName = editEmployee.FirstName;
            getEmployeeObject.LastName = editEmployee.LastName;
            getEmployeeObject.Email = editEmployee.Email;
            getEmployeeObject.DateOfBirth = editEmployee.DateOfBirth;

            await _context.SaveChangesAsync();
        }

        // Filter for: Employee Id, First Name, Last Name, Email
        public async Task<IEnumerable<Employee>> GetFilteredEmployees(string? searchString)
        {
            var employees = await GetAllEmployees();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int searchId))
                {
                    employees = employees.Where(e => e.Id == searchId).ToList();
                }
                else
                {
                    employees = employees.Where(e =>
                        e.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        e.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        e.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
            }

            return employees;
        }

        public async Task<bool> DeleteEmployeeById(long employeeId)
        {
            var employee = await GetEmployeeById(employeeId);

            if(employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> CheckEmailExist(string employeeEmail, long? employeeId)
        {

            if (employeeId == null)
            {
                // No Duplicate Email
                var employee = await GetEmployeeByEmail(employeeEmail);
                if (employee == false)
                {
                    return true;

                }
            }
            else if (employeeId != null)
            {

                // No Duplicate Email In Editing mode by selected user's data
                var checkEmployeeExistInEditMode = await _context.Employees.AnyAsync(w => w.Email == employeeEmail
                    && w.Id != employeeId);
               
                if (checkEmployeeExistInEditMode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
