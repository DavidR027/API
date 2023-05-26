using API.Models;
using API.ViewModels.Bookings;

namespace API.Contracts
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        //kel 4
        IEnumerable<BookingDetailVM> GetAllBookingDetail();
        BookingDetailVM GetBookingDetailByGuid(Guid guid);

        //kel 3
        IEnumerable<BookingDurationVM> GetBookingDuration();
    }
}

