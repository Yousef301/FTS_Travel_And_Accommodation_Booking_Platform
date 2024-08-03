﻿using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TABP.Application.Services.Interfaces;

namespace TABP.Application.Services.Implementations;

public class ImageService : IImageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    // private readonly ILogger<ImageService> _logger;

    public ImageService(IAmazonS3 s3Client, IConfiguration configuration, ILogger<ImageService> logger)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:BucketName"] ?? throw new ArgumentNullException(nameof(configuration));
        // _logger = logger;
    }

    public async Task UploadImagesAsync(List<IFormFile> files, Dictionary<string, object> configurations)
    {
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var fileName = $"{configurations["prefix"]}_{file.FileName}";

                if (configurations.ContainsKey("thumbnail") && (bool)configurations["thumbnail"])
                {
                    fileName = $"{configurations["prefix"]}_thumbnail{configurations["fileExtension"]}";
                }

                var fileKey = $"{configurations["folder"]}/{fileName}";

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    var putRequest = new PutObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = fileKey,
                        InputStream = stream,
                        ContentType = file.ContentType
                    };

                    try
                    {
                        await _s3Client.PutObjectAsync(putRequest);
                    }
                    catch (AmazonS3Exception ex)
                    {
                        // _logger.LogError($"Error uploading {file.FileName} to S3: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }
    }

    public async Task<byte[]?> GetImageAsync(string path)
    {
        try
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = path
            };

            using var response = await _s3Client.GetObjectAsync(getRequest);
            await using var responseStream = response.ResponseStream;
            using var memoryStream = new MemoryStream();
            await responseStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // _logger.LogWarning($"Image {name} not found in folder {folder}");
            return null;
        }
        catch (AmazonS3Exception ex)
        {
            // _logger.LogError($"Error retrieving {name} from S3: {ex.Message}", ex);
            throw;
        }
    }

    public async Task<IEnumerable<string>> GetSpecificImagesAsync(IEnumerable<string> specificPaths)
    {
        var listRequest = new ListObjectsV2Request
        {
            BucketName = _bucketName,
        };

        try
        {
            var listResponse = await _s3Client.ListObjectsV2Async(listRequest);
            var baseUrl = $"https://{_bucketName}.s3.amazonaws.com/";

            var filteredObjects = listResponse.S3Objects
                .Where(o => specificPaths.Contains(o.Key))
                .Select(o => baseUrl + o.Key);

            return filteredObjects.ToList();
        }
        catch (AmazonS3Exception ex)
        {
            // _logger.LogError($"Error listing images: {ex.Message}", ex);
            throw;
        }
    }

    public async Task<bool> DeleteImageAsync(string path)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = path
        };

        try
        {
            await _s3Client.DeleteObjectAsync(deleteRequest);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            // _logger.LogError($"Error deleting {path} from S3: {ex.Message}", ex);
            throw;
        }
    }
}