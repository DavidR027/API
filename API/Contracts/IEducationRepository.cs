﻿using API.Models;

namespace API.Contracts
{
    public interface IEducationRepository : IBaseRepository<Education>
    {
        IEnumerable<Education> GetByUniversityId(Guid universityId);
        Education GetByEmployeeId(Guid employeeId);

    }
}
