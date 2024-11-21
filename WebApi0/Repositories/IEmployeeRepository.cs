using Models;

namespace WebApi0.Models.Repositories // Remplacez "YourNamespace" par votre namespace réel
{
    public interface IEmployeeRepository
    {
        // Récupérer tous les employés
        Task<IEnumerable<Employee>> GetEmployees();

        // Récupérer un employé par ID
        Task<Employee> GetEmployee(int employeeId);

        // Ajouter un nouvel employé
        Task<Employee> AddEmployee(Employee employee);

        // Mettre à jour les informations d'un employé
        Task<Employee> UpdateEmployee(Employee employee);

        // Supprimer un employé par ID
        Task<Employee> DeleteEmployee(int employeeId);

        // Récupérer un employé par adresse e-mail
        Task<Employee> GetEmployeeByEmail(string email);
    Task<IEnumerable<Employee>> Search(string name, Gender? gender);
    }
}
