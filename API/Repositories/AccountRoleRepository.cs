using API.Contexts;
using API.Contracts;
using API.Models;
using System.Data;

namespace API.Repositories
{
    public class AccountRoleRepository : BaseRepository<AccountRole>, IAccountRoleRepository
    {
        public AccountRoleRepository(BookingManagementDbContext context) : base(context)
        {
        }
    }
}