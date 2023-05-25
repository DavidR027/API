using API.Contracts;
using API.Models;
using API.Repositories;
using API.ViewModels.Educations;
using API.ViewModels.Universities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EducationController : ControllerBase
{
    private readonly IEducationRepository _educationRepository;
    private readonly IMapper<Education, EducationVM> _mapper;
    public EducationController(IEducationRepository education, IMapper<Education, EducationVM> mapper)
    {
        _educationRepository = education;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var educations = _educationRepository.GetAll();
        if (!educations.Any())
        {
            return NotFound();
        }

        var resultConverted = educations.Select(_mapper.Map).ToList();

        return Ok(resultConverted);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var education = _educationRepository.GetByGuid(guid);
        if (education is null)
        {
            return NotFound();
        }

        var resultConverted = _mapper.Map(education);

        return Ok(resultConverted);
    }

    [HttpPost]
    public IActionResult Create(EducationVM educationVM)
    {
        var educationConverted = _mapper.Map(educationVM);

        var result = _educationRepository.Create(educationConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(EducationVM educationVM)
    {
        var educationConverted = _mapper.Map(educationVM);
        var isUpdated = _educationRepository.Update(educationConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _educationRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }
}


