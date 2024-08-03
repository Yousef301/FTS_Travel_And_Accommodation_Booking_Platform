using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Images.Cities.CreateCityImage;
using TABP.Application.Commands.Images.Cities.CreateCityThumbnail;
using TABP.Application.Commands.Images.Cities.DeleteCityImage;
using TABP.Application.Queries.Images.Cities.GetCityImageById;
using TABP.Application.Queries.Images.Cities.GetCityImages;
using TABP.Application.Queries.Images.Cities.GetCityThumbnail;


namespace TABP.Web.Controllers;

[ApiController]
[Route("api/cities/{cityId}/images")]
public class CityImagesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CityImagesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetImages(Guid cityId)
    {
        var images = await _mediator.Send(new GetCityImagesQuery
        {
            CityId = cityId
        });

        return Ok(images);
    }

    [HttpGet("{imageId:guid}")]
    public async Task<IActionResult> GetImage(Guid imageId)
    {
        var images = await _mediator.Send(new GetCityImageByIdQuery
        {
            ImageId = imageId
        });

        return File(images.Image, $"image/{images.Extension}");
    }

    [HttpPost]
    public async Task<IActionResult> UploadImages(Guid cityId,
        [FromForm(Name = "Images")] List<IFormFile> files)
    {
        await _mediator.Send(new CreateCityImageCommand
        {
            CityId = cityId,
            Images = files,
        });

        return Created();
    }

    [HttpGet("thumbnail")]
    public async Task<IActionResult> GetThumbnail(Guid cityId)
    {
        var images = await _mediator.Send(new GetCityThumbnailQuery
        {
            CityId = cityId
        });

        return File(images.Image, $"image/{images.Extension}");
    }

    [HttpPost("thumbnail")]
    public async Task<IActionResult> UploadThumbnail(Guid cityId,
        IFormFile image)
    {
        await _mediator.Send(new CreateCityThumbnailCommand
        {
            CityId = cityId,
            Image = image
        });

        return Created();
    }

    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(Guid imageId)
    {
        await _mediator.Send(new DeleteCityImageCommand
        {
            ImageId = imageId
        });

        return NoContent();
    }
}