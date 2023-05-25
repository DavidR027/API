using API.Contracts;
using API.Models;
using API.Repositories;
using API.ViewModels.Accounts;
using API.ViewModels.Roles;
using API.ViewModels.Rooms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Net.Mail;
using System.Net;
using API.Utility;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper<Account, AccountVM> _mapper;
    public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IMapper<Account, AccountVM> mapper)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return NotFound();
        }

        var resultConverted = accounts.Select(_mapper.Map).ToList();

        return Ok(resultConverted);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return NotFound();
        }

        var resultConverted = _mapper.Map(account);

        return Ok(resultConverted);
    }

    [HttpPost]
    public IActionResult Create(AccountVM accountVM)
    {
        var AccountConverted = _mapper.Map(accountVM);
        var result = _accountRepository.Create(AccountConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(AccountVM accountVM)
    {
        var AccountConverted = _mapper.Map(accountVM);
        var isUpdated = _accountRepository.Update(AccountConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _accountRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost("ForgotPassword"+"{email}")]
    public IActionResult UpdateResetPass(String email)
    {

        var getGuid = _employeeRepository.FindGuidByEmail(email);
        if (getGuid == null)
        {
            return NotFound("Akun tidak ditemukan");
        }

        var isUpdated = _accountRepository.UpdateOTP(getGuid);

        switch (isUpdated)
        {
            case 0:
                return BadRequest();
            default:
                var hasil = new AccountResetPasswordVM
                {
                    Email = email,
                    OTP = isUpdated
                };

                MailService mailService = new MailService();
                mailService.WithSubject("Kode OTP")
                           .WithBody("OTP anda adalah: " + isUpdated.ToString() + ".\n" +
                                     "Mohon kode OTP anda tidak diberikan kepada pihak lain" + ".\n" + "Terima kasih.")
                           .WithEmail(email)
                           .Send();

                return Ok(hasil);

        }


    }

}

