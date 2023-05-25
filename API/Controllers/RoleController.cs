using API.Contracts;
using API.Models;
using API.ViewModels.Roles;
using API.ViewModels.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper<Role, RoleVM> _mapper;
    public RoleController(IRoleRepository roleRepository, IMapper<Role, RoleVM> mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var roles = _roleRepository.GetAll();
        if (!roles.Any())
        {
            return NotFound();
        }

        var resultConverted = roles.Select(_mapper.Map).ToList();

        return Ok(resultConverted);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var role = _roleRepository.GetByGuid(guid);
        if (role is null)
        {
            return NotFound();
        }

        var resultConverted = _mapper.Map(role);

        return Ok(resultConverted);
    }

    [HttpPost]
    public IActionResult Create(RoleVM roleVM)
    {
        var roleConverted = _mapper.Map(roleVM);

        var result = _roleRepository.Create(roleConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(RoleVM roleVM)
    {
        var roleConverted = _mapper.Map(roleVM);
        var isUpdated = _roleRepository.Update(roleConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _roleRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }
}
