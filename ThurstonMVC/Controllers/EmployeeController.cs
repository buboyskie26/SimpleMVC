using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.CodeDom;
using ThurstonMVC.Contracts;
using ThurstonMVC.Data;
using ThurstonMVC.Models.Entities;
using ThurstonMVC.Models.ViewModel;

namespace ThurstonMVC.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IEmployee _employeeService;
        public EmployeeController(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }
            
        public async Task<IActionResult> Index(string? searchString)
        {
            try
            {
                var employees = await _employeeService.GetFilteredEmployees(searchString);

                ViewData["CurrentFilter"] = searchString;

                var employeeIndexView = new IndexEmployeeViewModel()
                {
                    Employee = employees,
                    SearchQuery = searchString
                };

                return View(employeeIndexView);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AddEmployeeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(AddEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, return the view with the current model to show validation errors
                return View("Create", model);
            }
            if (await _employeeService.CheckEmailExist(model.Email, null))
            {
                // Validation error in the FirstName field that will show-up on the Create MVC Page.
                ModelState.AddModelError("Email", "Email is already in use");
                return View("Create", model);
            }

            try
            {
                await _employeeService.Create(model);

                return RedirectToAction("Index", "Employee");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {
          
            var getEmployeeObject = await _employeeService.GetEmployeeById(id);
           
            // Employee not found base on Employee Id
            if (getEmployeeObject == null)
            {
                return View("NotFound");
            }

            var employee = new EditEmployeeViewModel()
            {
                Id = getEmployeeObject.Id,
                FirstName = getEmployeeObject.FirstName,
                LastName = getEmployeeObject.LastName,
                Email = getEmployeeObject.Email,
                DateOfBirth = getEmployeeObject.DateOfBirth,
            };

           
            return View(employee);
        }

        [HttpPost]
        public async Task<ActionResult> EditEmployee(
            EditEmployeeViewModel editEmployee)
        {

            if (!ModelState.IsValid)
            {
                // If the model state is invalid, return the view with the current model to show validation errors
                return View("Edit", editEmployee);
            }

            // 
            if (await _employeeService.CheckEmailExist(editEmployee.Email, editEmployee.Id))
            {
                // Validation error in the Email field that will show-up on the View Page
                ModelState.AddModelError("Email", "Email is already in use");

                return View("Edit", editEmployee);
            }

            // Get Employee Object base on the id parameter
            var getEmployeeObject = await _employeeService
                .GetEmployeeById(editEmployee.Id);

            if (getEmployeeObject == null)
            {
                return NotFound();
            }

            try
            {
                await _employeeService.Update(editEmployee);
                return RedirectToAction("Index", "Employee");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var isDeleted = await _employeeService.DeleteEmployeeById(id);

                if (isDeleted)
                {
                    // Employee deleted successfully
                    return Ok(); // Return HTTP 200 OK status
                }
                else
                {
                    // Unable to delete employee
                    return NotFound();
                }
            }
            catch (Exception)
            {
                // Handle any exceptions
                return StatusCode(500);
            }
        }
    }
}
