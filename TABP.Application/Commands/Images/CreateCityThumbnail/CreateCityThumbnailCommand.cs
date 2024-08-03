using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Commands.Images.CreateCityThumbnail;

public class CreateCityThumbnailCommand : IRequest
{
    public Guid CityId { get; set; }
    public IFormFile Image { get; set; }
}