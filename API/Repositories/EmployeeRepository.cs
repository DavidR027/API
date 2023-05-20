using API.Contexts;
using API.Contracts;
using API.Models;

namespace API.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BookingManagementDbContext context) : base(context)
        {
        }
    }
}

