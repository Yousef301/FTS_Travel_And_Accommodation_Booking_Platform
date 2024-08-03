namespace TABP.DAL.Interfaces.Repositories;

public interface IImageRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<string?> GetImagePathAsync(Guid id);
    Task<IEnumerable<string>> GetImagesPathAsync(Guid id);
    Task<string?> GetThumbnailPathAsync(Guid id);
    Task<T> CreateAsync(T cityImage);
    Task AddRangeAsync(IEnumerable<T> cityImages);
    Task DeleteAsync(Guid id);
}