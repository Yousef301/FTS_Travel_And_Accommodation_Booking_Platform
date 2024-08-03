using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface ICityImageRepository
{
    Task<IEnumerable<CityImage>> GetAsync();
    Task<CityImage?> GetByIdAsync(Guid id);
    Task<string?> GetCityImagePathAsync(Guid id);
    Task<IEnumerable<string>> GetCityImagesPathAsync(Guid id);
    Task<string?> GetCityThumbnailPathAsync(Guid id);
    Task<CityImage> CreateAsync(CityImage cityImage);
    Task AddRangeAsync(IEnumerable<CityImage> cityImages);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(CityImage cityImage);
    Task<bool> ExistsAsync(Expression<Func<CityImage, bool>> predicate);
}