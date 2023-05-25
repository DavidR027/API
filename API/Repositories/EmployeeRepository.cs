using API.Contexts;
using API.Contracts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BookingManagementDbContext context) : base(context)
        {
        }
        public Guid? FindGuidByEmail(string email)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(e => e.Email == email);
                if (employee == null)
                {
                    return null;
                }
                return employee.Guid;
            }
            catch
            {
                return null;
            }

        }
    }
}

