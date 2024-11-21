using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi0.Models.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<Department> GetDepartment(int departmentId);
    }
}
