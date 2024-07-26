using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface ICityImageRepository
{
    Task<IEnumerable<CityImage>> GetAsync();
    Task<CityImage?> GetByIdAsync(Guid id);
    Task<CityImage> CreateAsync(CityImage cityImage);
    Task DeleteAsync(CityImage cityImage);
    Task UpdateAsync(CityImage cityImage);
    Task<bool> ExistsAsync(Expression<Func<CityImage, bool>> predicate);
}