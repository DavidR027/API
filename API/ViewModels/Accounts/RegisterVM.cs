using API.Models;
using API.Utility;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels.Accounts
{
    public class RegisterVM
    {
        //public string NIK { get; set; }
        [Required(ErrorMessage = "First Name is Required")]
        //[Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTime BirthDate { get; set; }
        public GenderLevel Gender { get; set; }

        public DateTime HiringDate { get; set; }
        [EmailAddress]
        [NIKEmailPhoneValidation("Email")]
        public string Email { get; set; }
        [Phone]
        [NIKEmailPhoneValidation("Phone Number")]
        public string PhoneNumber { get; set; }

        public string Major { get; set; }

        public string Degree { get; set; }
        [Range(0, 4, ErrorMessage = "GPA must be between 0 to 4")]
        public float GPA { get; set; }

        //public Guid UniversityGuid { get; set; }

        public string UniversityCode { get; set; }

        public string UniversityName { get; set; }
        [PasswordValidation(ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, 1 number, 1 symbol, and a minimum of 6 characters")]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        // public University? University { get; set; }


    }
}
