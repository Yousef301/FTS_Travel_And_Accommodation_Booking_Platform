using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Services.Interfaces;

public interface IImageService
{
    Task UploadImagesAsync(List<IFormFile> files, Dictionary<string, object> configurations);
    Task<byte[]?> GetImageAsync(string path);
    Task<string> GetImageUrlAsync(string imageKey);
    Task<object> GetImagesUrlsAsync<T>(IEnumerable<string> specificPaths);
    Task<bool> DeleteImageAsync(string path);
}