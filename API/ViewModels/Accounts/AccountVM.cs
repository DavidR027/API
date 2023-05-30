using API.Utility;

namespace API.ViewModels.Accounts
{
    public class AccountVM
    {
        public Guid? Guid { get; set; }
        [PasswordValidation(ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, 1 number, 1 symbol, and a minimum of 6 characters")]
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public int OTP { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
