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

/*        public Account GetByEmployeeId(Guid? employeeId)
        {
            return _context.Set<Account>().FirstOrDefault(a => a.Guid == employeeId);
        }*/

        public int UpdateOTP(Guid? employeeId)
        {
            var account = new Account();
            account = _context.Set<Account>().FirstOrDefault(a => a.Guid == employeeId);
            //Generate OTP
            Random rnd = new Random();
            var getOtp = rnd.Next(100000, 999999);
            account.OTP = getOtp;

            //Add 5 minutes to expired time
            account.ExpiredTime = DateTime.Now.AddMinutes(5);
            account.IsUsed = false;
            try
            {
                var check = Update(account);
                

                if (!check)
                {
                    return 0;
                }
                return getOtp;
            }
            catch
            {
                return 0;
            }
        }
    }
}


