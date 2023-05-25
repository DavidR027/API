using API.Contracts;
using API.Models;
using API.ViewModels.Educations;
using API.ViewModels.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper<Room, RoomVM> _mapper;
    public RoomController(IRoomRepository roomRepository, IMapper<Room, RoomVM> mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var room = _roomRepository.GetAll();
        if (!room.Any())
        {
            return NotFound();
        }

        var resultConverted = room.Select(_mapper.Map).ToList();

        return Ok(resultConverted);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var room = _roomRepository.GetByGuid(guid);
        if (room is null)
        {
            return NotFound();
        }

        var resultConverted = _mapper.Map(room);

        return Ok(resultConverted);
    }

    [HttpPost]
    public IActionResult Create(RoomVM roomVM)
    {
        var roomConverted = _mapper.Map(roomVM);

        var result = _roomRepository.Create(roomConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(RoomVM roomVM)
    {
        var roomConverted = _mapper.Map(roomVM);
        var isUpdated = _roomRepository.Update(roomConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _roomRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }
}