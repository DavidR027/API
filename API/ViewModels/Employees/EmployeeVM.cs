using API.Utility;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels.Employees
{
    public class EmployeeVM
    {
        public Guid? Guid { get; set; }
        [NIKEmailPhoneValidation(nameof(NIK))]
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderLevel Gender { get; set; }
        public DateTime HiringDate { get; set; }
        [EmailAddress]
        [NIKEmailPhoneValidation(nameof(Email))]
        public string Email { get; set; }
        [Phone]
        [NIKEmailPhoneValidation(nameof(PhoneNumber))]
        public string PhoneNumber { get; set; }
    }
}
