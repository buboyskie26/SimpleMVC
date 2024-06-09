using ThurstonMVC.Models.Entities;
using ThurstonMVC.Models.ViewModel;

namespace ThurstonMVC.Contracts
{
    public interface IEmployee
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<IEnumerable<Employee>> GetFilteredEmployees(string? searchString);

        Task<Employee> GetEmployeeById(long employeeId);
        Task<bool> GetEmployeeByEmail(string employeeEmail);
        
        Task<bool> DeleteEmployeeById(long employeeId);
        Task<bool> CheckEmailExist(string employeeEmail, long? employeeId);
        
        Task Create(AddEmployeeViewModel employeeCreateModel);
        Task Update(EditEmployeeViewModel editEmployee);
        
    }
}
