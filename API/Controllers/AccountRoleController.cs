using API.Contracts;
using API.Models;
using API.ViewModels.AccountRoles;
using API.ViewModels.Accounts;
using API.ViewModels.Others;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AccountRoleController : BaseController<AccountRole, AccountRoleVM>
{
    public AccountRoleController(IAccountRoleRepository accountRoleRepository, IMapper<AccountRole, AccountRoleVM> mapper)
        : base(accountRoleRepository, mapper)
    {
    }
}
/*
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
            return NotFound(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found",
                Data = null
            });
        }

        var resultConverted = accountRoles.Select(_mapper.Map).ToList();

        return Ok(new ResponseVM<List<AccountRoleVM>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success",
            Data = resultConverted
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);
        if (accountRole is null)
        {
            return NotFound(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Account Role not found",
                Data = null
            });
        }

        var resultConverted = _mapper.Map(accountRole);

        return Ok(new ResponseVM<AccountRoleVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success",
            Data = resultConverted
        });
    }

    [HttpPost]
    public IActionResult Create(AccountRoleVM accountRoleVM)
    {
        var AccountConverted = _mapper.Map(accountRoleVM);
        var result = _accountRoleRepository.Create(AccountConverted);
        if (result is null)
        {
            return BadRequest(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Create Failed",
                Data = null
            });
        }

        return Ok(new ResponseVM<AccountRole>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Create Success",
            Data = result
        });
    }

    [HttpPut]
    public IActionResult Update(AccountRoleVM accountRoleVM)
    {
        var AccountConverted = _mapper.Map(accountRoleVM);
        var isUpdated = _accountRoleRepository.Update(AccountConverted);
        if (!isUpdated)
        {
            return BadRequest(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Update Failed",
                Data = null
            });
        }

        return Ok(new ResponseVM<AccountRole>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Update Success",
            Data = null
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _accountRoleRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Delete Failed",
                Data = null
            });
        }

        return Ok(new ResponseVM<AccountRole>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Delete Success",
            Data = null
        });
    }
}
*/
