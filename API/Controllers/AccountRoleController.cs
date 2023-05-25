using API.Contracts;
using API.Models;
using API.ViewModels.AccountRoles;
using API.ViewModels.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IMapper<AccountRole, AccountRoleVM> _mapper;
    public AccountRoleController(IAccountRoleRepository accountRoleRepository, IMapper<AccountRole, AccountRoleVM> mapper)
    {
        _accountRoleRepository = accountRoleRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var accountRoles = _accountRoleRepository.GetAll();
        if (!accountRoles.Any())
        {
            return NotFound();
        }

        var resultConverted = accountRoles.Select(_mapper.Map).ToList();

        return Ok(resultConverted);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);
        if (accountRole is null)
        {
            return NotFound();
        }

        var resultConverted = _mapper.Map(accountRole);

        return Ok(resultConverted);
    }

    [HttpPost]
    public IActionResult Create(AccountRoleVM accountRoleVM)
    {
        var AccountConverted = _mapper.Map(accountRoleVM);
        var result = _accountRoleRepository.Create(AccountConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(AccountRoleVM accountRoleVM)
    {
        var AccountConverted = _mapper.Map(accountRoleVM);
        var isUpdated = _accountRoleRepository.Update(AccountConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _accountRoleRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }
}

