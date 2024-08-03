﻿using Microsoft.AspNetCore.Http;

namespace TABP.Application.Services.Interfaces;

public interface IImageService
{
    Task UploadImagesAsync(List<IFormFile> files, Dictionary<string, object> configurations);
    Task<byte[]?> GetImageAsync(string path);
    Task<IEnumerable<string>> GetSpecificImagesAsync(IEnumerable<string> paths);
    Task<bool> DeleteImageAsync(string path);
}