using API.Contexts;
using API.Contracts;
using API.Models;
using System.Data;

namespace API.Repositories
{
    public class EducationRepository : BaseRepository<Education>, IEducationRepository
    {
        public EducationRepository(BookingManagementDbContext context) : base(context)
        {

        }
        public IEnumerable<Education> GetByUniversityId(Guid universityId)
        {
            return _context.Set<Education>().Where(e => e.UniversityGuid == universityId);
        }
    }
}



