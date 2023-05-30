using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Repositories;
using API.ViewModels.Employees;

namespace API.Contracts
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        bool CheckEmailAndPhoneAndNIK(string value);
        int CreateWithValidate(Employee employee);

        //kel 1
        IEnumerable<MasterEmployeeVM> GetAllMasterEmployee();
        MasterEmployeeVM? GetMasterEmployeeByGuid(Guid guid);

        //kel 5 & 6
        Guid? FindGuidByEmail(string email);
    }
}
