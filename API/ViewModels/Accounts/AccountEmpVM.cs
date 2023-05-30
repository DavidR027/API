using System.ComponentModel.DataAnnotations;

namespace API.ViewModels.Accounts
{
    public class AccountEmpVM
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}