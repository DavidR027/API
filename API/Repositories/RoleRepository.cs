using API.Contexts;
using API.Contracts;
using API.Models;
using System.Data;

namespace API.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(BookingManagementDbContext context) : base(context)
        {
        }
    }
}

