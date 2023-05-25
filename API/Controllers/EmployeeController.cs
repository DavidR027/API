using API.Contracts;
using API.Models;
using API.Repositories;
using API.ViewModels.Accounts;
using API.ViewModels.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper<Employee, EmployeeVM> _mapper;
    public EmployeeController(IEmployeeRepository employeeRepository, IMapper<Employee, EmployeeVM> mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var employees = _employeeRepository.GetAll();
        if (!employees.Any())
        {
            return NotFound();
        }

        var resultConverted = employees.Select(_mapper.Map).ToList();

        return Ok(resultConverted);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return NotFound();
        }

        var resultConverted = _mapper.Map(employee);

        return Ok(resultConverted);
    }

    [HttpPost]
    public IActionResult Create(EmployeeVM employeeVM)
    {
        var EmployeeConverted = _mapper.Map(employeeVM);
        var result = _employeeRepository.Create(EmployeeConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(EmployeeVM employeeVM)
    {
        var EmployeeConverted = _mapper.Map(employeeVM);
        var isUpdated = _employeeRepository.Update(EmployeeConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _employeeRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }
}
