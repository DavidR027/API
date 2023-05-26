using API.Contracts;
using API.Models;
using API.Repositories;
using API.ViewModels.Accounts;
using API.ViewModels.Bookings;
using API.ViewModels.Educations;
using API.ViewModels.Others;
using API.ViewModels.Universities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : BaseController<University, UniversityVM>
{
    private readonly IUniversityRepository _universityRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IMapper<University, UniversityVM> _mapper;
    private readonly IMapper<Education, EducationVM> _educationVMMapper;
    public UniversityController(IUniversityRepository universityRepository, IEducationRepository educationRepository, IMapper<University, UniversityVM> mapper, IMapper<Education, EducationVM> educationMapper) : base(universityRepository, mapper)
    {
        _universityRepository = universityRepository;
        _educationRepository = educationRepository;
        _mapper = mapper;
        _educationVMMapper = educationMapper;
    }

    [HttpGet("WithEducation")]
    public IActionResult GetAllWithEducation()
    {
        var universities = _universityRepository.GetAll();
        if (!universities.Any())
        {
            return NotFound(new ResponseVM<UniversityVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "University not found",
                Data = null
            });
        }

        var results = new List<UniversityEducationVM>();
        foreach (var university in universities)
        {
            var education = _educationRepository.GetByUniversityId(university.Guid);
            var educationMapped = education.Select(_educationVMMapper.Map);

            var result = new UniversityEducationVM
            {
                Guid = university.Guid,
                Code = university.Code,
                Name = university.Name,
                Educations = educationMapped
            };

            results.Add(result);
        }

        return Ok(new ResponseVM<List<UniversityEducationVM>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success",
            Data = results
        });
    }

/*    [HttpGet]
    public IActionResult GetAll()
    {
        var universities = _universityRepository.GetAll();
        if (!universities.Any())
        {
            return NotFound();
        }

        *//*var univeritiesConverted = new List<UniversityVM>();
        foreach (var university in universities) {
            var result = UniversityVM.ToVM(university);
            univeritiesConverted.Add(result);
        }*//*

        var data = universities.Select(_mapper.Map).ToList();

        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var university = _universityRepository.GetByGuid(guid);
        if (university is null)
        {
            return NotFound();
        }

        var data = _mapper.Map(university);

        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create(UniversityVM universityVM)
    {
        var universityConverted = _mapper.Map(universityVM);

        var result = _universityRepository.Create(universityConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(UniversityVM universityVM)
    {
        var universityConverted = _mapper.Map(universityVM);

        var isUpdated = _universityRepository.Update(universityConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _universityRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }*/
}