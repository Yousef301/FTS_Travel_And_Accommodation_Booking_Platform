using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IHotelImageRepository
{
    Task<HotelImage?> GetByIdAsync(Guid id);
    Task<string?> GetImagePathAsync(Guid id);
    Task<IEnumerable<string>> GetImagesPathAsync(Guid id);
    Task<string?> GetThumbnailPathAsync(Guid id);
    Task<HotelImage> CreateAsync(HotelImage hotelImage);
    Task AddRangeAsync(IEnumerable<HotelImage> hotelImages);
    Task DeleteAsync(Guid id);
}