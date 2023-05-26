using API.Contexts;
using API.Contracts;
using API.Models;
using API.ViewModels.Accounts;
using API.ViewModels.Bookings;
using System.Data;

namespace API.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(BookingManagementDbContext context) : base(context)
        {
        }

        //kel 3
        private int CalculateBookingDuration(DateTime startDate, DateTime endDate)
        {
            int totalDays = 0; // Untuk menghitung hari
            DateTime currentDate = startDate.Date; // Perhitungan tergantung dari tanggal yang digunakan

            while (currentDate <= endDate.Date)
            {
                // Mengecek apakah currentDate adalah hari kerja (Selain sabtu dan minggu) 
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    // Jika currentDate hari kerja, totalDays bertambah
                    totalDays++;
                }
                currentDate = currentDate.AddDays(1); // untuk maju ke tanggal berikutnya.
            }

            return totalDays;
        }

        //kel 3
        public IEnumerable<BookingDurationVM> GetBookingDuration()
        {
            /* Menampung semua data room di var rooms*/
            var rooms = _context.Rooms.ToList();

            /* Mengambil semua data booking*/
            var bookings = GetAll();

            /* Melakukan instance/membuat object untuk setiap pemesanan yang memenuhi kondisi diatas..
               Pada part ini, value RoomName akan diisi dengan nama Room yang dicari berdasarkan RoomGuid. */
            var bookingduration = bookings.Select(b => new BookingDurationVM
            {
                RoomName = rooms.FirstOrDefault(r => r.Guid == b.RoomGuid)?.Name, // Di set menjadi (?) untuk memastikan tidak terjadi kesalahan ketika objek tidak ditemukan, sehingga valuenya akan otomatis NULL
                DurationOfBooking = CalculateBookingDuration(b.StartDate, b.EndDate)
            });

            return bookingduration;
        }

        //get by guid kel 4
        public BookingDetailVM GetBookingDetailByGuid(Guid guid)
        {
            var booking = GetByGuid(guid);
            var employee = _context.Employees.Find(booking.EmployeeGuid);
            var room = _context.Rooms.Find(booking.RoomGuid);

            var bookingDetail = new BookingDetailVM
            {
                Guid = booking.Guid,
                BookedNIK = employee.NIK,
                Fullname = employee.FirstName + " " + employee.LastName,
                RoomName = room.Name,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status,
                Remarks = booking.Remarks
            };

            return bookingDetail;
        }

        //get all kel 4
        public IEnumerable<BookingDetailVM> GetAllBookingDetail()
        {
            var bookings = GetAll();
            var employees = _context.Employees.ToList();
            var rooms = _context.Rooms.ToList();
            var results = new List<BookingDetailVM>();
            var bookingDetail = from b in bookings
                                join e in employees on b.EmployeeGuid equals e.Guid
                                join r in rooms on b.RoomGuid equals r.Guid
                                select new
                                {
                                    b.Guid,
                                    e.NIK,
                                    BookedBy = e.FirstName + " " + e.LastName,
                                    r.Name,
                                    b.StartDate,
                                    b.EndDate,
                                    b.Status,
                                    b.Remarks
                                };



            foreach (var booking in bookingDetail)
            {
                var result = new BookingDetailVM
                {
                    Guid = booking.Guid,
                    BookedNIK = booking.NIK,
                    Fullname = booking.BookedBy,
                    RoomName = booking.Name,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    Status = booking.Status,
                    Remarks = booking.Remarks
                };

                results.Add(result);

            }
            return results;
        }
    }
}




