using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TABP.Application.Services.Interfaces;
using TABP.Domain.Services.Interfaces;

namespace TABP.Application.Services.Implementations.AWS;

public class S3ImageService : IImageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly ILogger _logger;

    public S3ImageService(IAmazonS3 s3Client,
        ILogger<S3ImageService> logger,
        ISecretsManagerService secretsManagerService)
    {
        _s3Client = s3Client;
        _logger = logger;

        var secrets = secretsManagerService.GetSecretAsDictionaryAsync("dev_fts_aws").Result
                      ?? throw new ArgumentNullException(nameof(secretsManagerService));

        _bucketName = secrets["ImagesBucket"];
    }

    public async Task UploadImagesAsync(List<IFormFile> files,
        Dictionary<string, object> configurations)
    {
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var fileName = $"{configurations["prefix"]}_{file.FileName}";
                var fileKey = $"{configurations["folder"]}/{fileName}".Replace(' ', '_');

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
                        _logger.LogError($"Error uploading {file.FileName} to S3: {ex.Message}", ex);
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
            return null;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError($"Error retrieving '{path}' from S3: {ex.Message}", ex);
            throw;
        }
    }

    public async Task<string> GetImageUrlAsync(string imageKey)
    {
        var listRequest = new ListObjectsV2Request
        {
            BucketName = _bucketName,
        };

        try
        {
            var listResponse = await _s3Client.ListObjectsV2Async(listRequest);

            var baseUrl = $"https://{_bucketName}.s3.amazonaws.com/";

            var imageObject = listResponse.S3Objects
                .FirstOrDefault(o => o.Key == imageKey);

            if (imageObject == null)
            {
                return string.Empty;
            }

            return baseUrl + imageObject.Key;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError($"Error getting image URL: {ex.Message}", ex);
            throw;
        }
    }


    public async Task<object> GetImagesUrlsAsync<T>(IEnumerable<string> specificPaths)
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
                .Select(o => new Dictionary<string, string>
                {
                    { "url", baseUrl + o.Key },
                    { "uploadDate", o.LastModified.ToString("yyyy-MM-ddTHH:mm:ssZ") }
                });

            if (typeof(T) == typeof(List<string>))
            {
                var urls = filteredObjects.Select(d => d["url"]).ToList();
                return urls;
            }
            else if (typeof(T) == typeof(IEnumerable<Dictionary<string, string>>))
            {
                return filteredObjects;
            }
            else
            {
                throw new InvalidOperationException("Unsupported type parameter.");
            }
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError($"Error listing images: {ex.Message}", ex);
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
            _logger.LogError($"Error deleting {path} from S3: {ex.Message}", ex);
            throw;
        }
    }
}