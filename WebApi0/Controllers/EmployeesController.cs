using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi0.Models.Repositories;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    // Injection de dépendance du service IEmployeeRepository
    private readonly IEmployeeRepository employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<ActionResult> GetEmployees()
    {
        try
        {
            return Ok(await employeeRepository.GetEmployees());
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error retrieving data from the database");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        try
        {
            var result = await employeeRepository.GetEmployee(id);
            if (result == null) return NotFound();
            return result;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error retrieving data from the database");
        }
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<Employee>> GetEmployeeByEmail(string email)
    {
        try
        {
            var employee = await employeeRepository.GetEmployeeByEmail(email);
            if (employee == null) return NotFound();
            return employee;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error retrieving data from the database");
        }
    }

    // Méthode de recherche ajoutée
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
    {
        try
        {
            var result = await employeeRepository.Search(name, gender);
            if (result.Any())
                return Ok(result);
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error retrieving data from the database");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
    {
        try
        {
            if (employee == null)
                return BadRequest("Employee object is null.");

            // Vérifier si l'email de l'employé existe déjà
            var existingEmployee = await employeeRepository.GetEmployeeByEmail(employee.Email);
            if (existingEmployee != null)
            {
                // Ajouter une erreur de validation pour le modèle
                ModelState.AddModelError("email", "Employee email already in use");
                return BadRequest(ModelState);
            }

            // Ajouter le nouvel employé
            var createdEmployee = await employeeRepository.AddEmployee(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.EmployeeId }, createdEmployee);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error creating new employee record: " + ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Employee>> UpdateEmployee(int id, [FromBody] Employee employee)
    {
        try
        {
            if (id != employee.EmployeeId)
                return BadRequest("Employee ID mismatch");

            var employeeToUpdate = await employeeRepository.GetEmployee(id);
            if (employeeToUpdate == null)
                return NotFound($"Employee with Id = {id} not found");

            var updatedEmployee = await employeeRepository.UpdateEmployee(employee);
            return Ok(updatedEmployee);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error updating data");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Employee>> DeleteEmployee(int id)
    {
        try
        {
            var employeeToDelete = await employeeRepository.GetEmployee(id);
            if (employeeToDelete == null)
                return NotFound($"Employee with Id = {id} not found");

            await employeeRepository.DeleteEmployee(id);
            return NoContent(); // Successfully deleted, no content to return
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error deleting data");
        }
    }
}
