using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IHotelImageRepository
{
    Task<IEnumerable<HotelImage>> GetAsync();
    Task<HotelImage?> GetByIdAsync(Guid id);
    Task<HotelImage> CreateAsync(HotelImage hotelImage);
    Task DeleteAsync(HotelImage hotelImage);
    Task UpdateAsync(HotelImage hotelImage);
    Task<bool> ExistsAsync(Expression<Func<HotelImage, bool>> predicate);
}