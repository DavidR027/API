using API.Contexts;
using API.Contracts;
using API.Models;
using System.Data;

namespace API.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(BookingManagementDbContext context) : base(context)
        {
        }
    }
}


