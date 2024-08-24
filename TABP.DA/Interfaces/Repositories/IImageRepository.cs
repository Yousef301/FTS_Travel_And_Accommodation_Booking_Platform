using System.Linq.Expressions;

namespace TABP.DAL.Interfaces.Repositories;

public interface IImageRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate);
    Task<string?> GetImagePathAsync(Guid imageId, Guid cityId);
    Task<IEnumerable<string>> GetImagesPathAsync(Guid id);
    Task<T> CreateAsync(T cityImage);
    Task AddRangeAsync(IEnumerable<T> cityImages);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}