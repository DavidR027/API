﻿using API.Contexts;
using API.Contracts;
using API.Models;
using API.ViewModels.Rooms;

namespace API.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public RoomRepository(BookingManagementDbContext context,
            IBookingRepository bookingRepository,
            IEmployeeRepository employeeRepository) : base(context)
        {
            _bookingRepository = bookingRepository;
            _employeeRepository = employeeRepository;
        }

        //kel 4
        public IEnumerable<RoomBookedTodayVM> GetAvailableRoom()
        {
            try
            {
                //get all data from booking and rooms
                var booking = _bookingRepository.GetAll();
                var rooms = GetAll();

                var startToday = DateTime.Today;
                var endToday = DateTime.Today.AddHours(23).AddMinutes(59);

                var roomUse = rooms.Join(booking, Room => Room.Guid, booking => booking.RoomGuid, (Room, booking) => new { Room, booking })
                        .Select(joinResult => new {
                            joinResult.Room.Name,
                            joinResult.Room.Floor,
                            joinResult.Room.Capacity,
                            joinResult.booking.StartDate,
                            joinResult.booking.EndDate
                        }
                 );

                var roomUseTodays = new List<RoomBookedTodayVM>();


                foreach (var room in roomUse)
                {
                    if ((room.StartDate < startToday && room.EndDate < startToday) || (room.StartDate > startToday && room.EndDate > endToday))
                    {
                        var roomDay = new RoomBookedTodayVM
                        {
                            RoomName = room.Name,
                            Floor = room.Floor,
                            Capacity = room.Capacity
                        };
                        roomUseTodays.Add(roomDay);
                    }
                }
                return roomUseTodays;
            }

            catch
            {
                return null;

            }
        }

        //kel 1
        public IEnumerable<MasterRoomVM> GetByDate(DateTime dateTime)
        {
            var rooms = GetAll();
            var bookings = _bookingRepository.GetAll();
            var employees = _employeeRepository.GetAll();

            var usedRooms = new List<MasterRoomVM>();

            foreach (var room in rooms)
            {
                var booking = bookings.FirstOrDefault(b => b.RoomGuid == room?.Guid && b.StartDate <= dateTime && b.EndDate >= dateTime);
                if (booking != null)
                {
                    var employee = employees.FirstOrDefault(e => e.Guid == booking.EmployeeGuid);

                    var result = new MasterRoomVM
                    {
                        BookedBy = employee.FirstName + " " + employee?.LastName,
                        Status = booking.Status.ToString(),
                        RoomName = room.Name,
                        Floor = room.Floor,
                        Capacity = room.Capacity,
                        StartDate = booking.StartDate,
                        EndDate = booking.EndDate,

                    };

                    usedRooms.Add(result);
                }
            }

            return usedRooms;
        }

        //kel 1
        public IEnumerable<RoomUsedVM> GetCurrentlyUsedRooms()
        {
            var rooms = GetAll();
            var bookings = _bookingRepository.GetAll();
            var employees = _employeeRepository.GetAll();

            var usedRooms = new List<RoomUsedVM>();

            var currentTime = DateTime.Now;

            foreach (var room in rooms)
            {
                var booking = bookings.FirstOrDefault(b => b.RoomGuid == room?.Guid && b.StartDate <= currentTime && b.EndDate >= currentTime);
                if (booking != null)
                {
                    var employee = employees.FirstOrDefault(e => e.Guid == booking.EmployeeGuid);

                    var result = new RoomUsedVM
                    {
                        RoomName = room.Name,
                        Status = booking.Status.ToString(),
                        Floor = room.Floor,
                        BookedBy = employee.FirstName + " " + employee?.LastName,
                    };

                    usedRooms.Add(result);
                }
            }
            return usedRooms;
        }
    }
}

