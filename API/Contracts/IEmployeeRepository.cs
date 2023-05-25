using API.Models;

namespace API.Contracts
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Guid? FindGuidByEmail(string email);
    }
}
