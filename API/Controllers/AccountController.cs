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
using API.ViewModels.Login;
using API.ViewModels.Universities;
using API.ViewModels.Others;
using System.Net.NetworkInformation;
using API.ViewModels.AccountRoles;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AccountController : BaseController<Account, AccountVM>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly IMapper<Account, AccountVM> _mapper;
    private readonly IMapper<Account, ChangePasswordVM> _changePasswordMapper;
    public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IEmailService emailService, ITokenService tokenService, IMapper<Account, AccountVM> mapper, IMapper<Account, ChangePasswordVM> changePasswordMapper) : base(accountRepository, mapper)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _tokenService = tokenService;
        _emailService = emailService;
        _mapper = mapper;
        _changePasswordMapper = changePasswordMapper;
    }


    //kel 3
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login(LoginVM loginVM)
    {
        var account = _accountRepository.Login(loginVM);

        if (account == null)
        {
            return NotFound(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Account not found",
                Data = null
            });
        }

        if (!Hashing.ValidatePassword(loginVM.Password, account.Password))
        {
            return BadRequest(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Password Invalid",
                Data = null
            });
        }

        var employee = _accountRepository.GetEmployeeByEmail(account.Email);
        if (employee == null)
        {
            return NotFound(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email not found",
                Data = null
            });
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, employee.NIK),
            new(ClaimTypes.Name, $"{employee.FirstName} {employee.LastName}"),
            new(ClaimTypes.Email, employee.Email)
        };

        var roles = _accountRepository.GetRoles(employee.Guid);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = _tokenService.GenerateToken(claims);

        return Ok(new ResponseVM<string>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Login Success",
            Data = token
        });

    }


    [HttpPost("ExtractToken/{token}")]
    public IActionResult ExtractToken(string token)
    {

        var extract = _tokenService.ExtractClaimsFromJwt(token);

        if (extract == null || extract.NameIdentifier == null)
        {
            return NotFound(new ResponseVM<ClaimVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Token not found",
                Data = null
            });
        }

        return Ok(new ResponseVM<ClaimVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Extract Success",
            Data = extract
        });

    }


    //kel 2
    [HttpPost("Register")]
    [AllowAnonymous]
    public IActionResult Register(RegisterVM registerVM)
    {

        var result = _accountRepository.Register(registerVM);
        switch (result)
        {
            case 0:
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Registration Failed",
                    Data = null
                });
            case 1:
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Email already exists",
                    Data = null
                });
            case 2:
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Phone number already exists",
                    Data = null
                });
            case 3:
                return Ok(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Registration Success",
                    Data = null
                });
        }

        return BadRequest(new ResponseVM<AccountVM>
        {
            Code = StatusCodes.Status400BadRequest,
            Status = HttpStatusCode.BadRequest.ToString(),
            Message = "Registration Failed",
            Data = null
        });

    }


    //kel 5
    [HttpPost("ForgotPassword/{email}")]
    [AllowAnonymous]
    public IActionResult UpdateResetPass(String email)
    {

        var getGuid = _employeeRepository.FindGuidByEmail(email);
        if (getGuid == null)
        {
            return NotFound(new ResponseVM<AccountVM>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Email not Found",
            Data = null
        });
        }

        var isUpdated = _accountRepository.UpdateOTP(getGuid);

        switch (isUpdated)
        {
            case 0:
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP Failed to Generate",
                    Data = null
                });
            default:
                var hasil = new AccountResetPasswordVM
                {
                    Email = email,
                    OTP = isUpdated
                };

                _emailService.SetEmail(email)
                             .SetSubject("Forgot Password")
                             .SetHtmlMessage($"Your OTP is {isUpdated}")
                             .SendEmailAsync();

                return Ok(new ResponseVM<AccountResetPasswordVM>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "OTP Successfully Sent to Email",
                    Data = hasil
                });

        }


    }

    //kel 6
    [HttpPost("ChangePassword")]
    [AllowAnonymous]
    public IActionResult ChangePassword(ChangePasswordVM changePasswordVM)
    {
        // Cek apakah email dan OTP valid
        var account = _employeeRepository.FindGuidByEmail(changePasswordVM.Email);
        var changePass = _accountRepository.ChangePasswordAccount(account, changePasswordVM);
        switch (changePass)
        {
            case 0:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Unable to Change Password",
                    Data = null
                });
            case 1:
                return Ok(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Password has been changed successfully",
                    Data = null
                });
            case 2:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Invalid OTP",
                    Data = null
                });
            case 3:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP has already been used",
                    Data = null
                });
            case 4:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP expired",
                    Data = null
                });
            case 5:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Wrong Password, Not the Same",
                    Data = null
                });
            default:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Unable to Change Password",
                    Data = null
                });
        }

    }

}

