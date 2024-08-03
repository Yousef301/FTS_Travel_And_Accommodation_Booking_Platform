using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Commands.Images.CreateCityImage;

public class CreateCityImageCommand : IRequest
{
    public Guid CityId { get; set; }
    public List<IFormFile> Images { get; set; }
}