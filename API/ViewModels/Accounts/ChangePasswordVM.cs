using API.Utility;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels.Accounts
{
    public class ChangePasswordVM
    {
        [EmailAddress]
        public string Email { get; set; }
        public int OTP { get; set; }
        [PasswordValidation(ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, 1 number, 1 symbol, and a minimum of 6 characters")]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
