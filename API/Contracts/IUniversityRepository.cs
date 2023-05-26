using API.Models;

namespace API.Contracts
{
    public interface IUniversityRepository : IBaseRepository<University>
    {
        University CreateWithValidate(University university);
    }
}
