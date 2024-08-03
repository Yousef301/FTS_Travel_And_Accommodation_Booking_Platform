using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Commands.Images.Cities.CreateCityThumbnail;

public class CreateCityThumbnailCommand : IRequest
{
    public Guid CityId { get; set; }
    public IFormFile Image { get; set; }
}