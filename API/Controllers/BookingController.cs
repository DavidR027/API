using API.Contracts;
using API.Models;
using API.ViewModels.Accounts;
using API.ViewModels.Bookings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper<Booking, BookingVM> _mapper;
    public BookingController(IBookingRepository booking, IMapper<Booking, BookingVM> mapper)
    {
        _bookingRepository = booking;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var bookings = _bookingRepository.GetAll();
        if (!bookings.Any())
        {
            return NotFound();
        }

        var resultConverted = bookings.Select(_mapper.Map).ToList();

        return Ok(resultConverted);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null)
        {
            return NotFound();
        }

        var resultConverted = _mapper.Map(booking);

        return Ok(resultConverted);
    }

    [HttpPost]
    public IActionResult Create(BookingVM bookingVM)
    {
        var BookingConverted = _mapper.Map(bookingVM);
        var result = _bookingRepository.Create(BookingConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(BookingVM bookingVM)
    {
        var BookingConverted = _mapper.Map(bookingVM);
        var isUpdated = _bookingRepository.Update(BookingConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _bookingRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }
}


