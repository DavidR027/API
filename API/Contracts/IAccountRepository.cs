using API.Models;
using API.ViewModels.Accounts;
using API.ViewModels.Login;

namespace API.Contracts
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        //kel 2
        int Register(RegisterVM registerVM);

        //kel 3
        AccountEmpVM Login(LoginVM loginVM);

        //kel 5
        int UpdateOTP(Guid? employeeId);

        //kel 6
        public int ChangePasswordAccount(Guid? employeeId, ChangePasswordVM changePasswordVM);
    }
}