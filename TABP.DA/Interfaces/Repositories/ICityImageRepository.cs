using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface ICityImageRepository
{
    Task<IEnumerable<CityImage>> GetAsync();
    Task<CityImage?> GetByIdAsync(Guid id);
    Task<string?> GetImagePathAsync(Guid id);
    Task<IEnumerable<string>> GetImagesPathAsync(Guid id);
    Task<string?> GetThumbnailPathAsync(Guid id);
    Task<CityImage> CreateAsync(CityImage cityImage);
    Task AddRangeAsync(IEnumerable<CityImage> cityImages);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(CityImage cityImage);
    Task<bool> ExistsAsync(Expression<Func<CityImage, bool>> predicate);
}