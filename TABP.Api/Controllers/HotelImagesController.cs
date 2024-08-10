using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Images.Hotels.CreateHotelImage;
using TABP.Application.Commands.Images.Hotels.CreateHotelThumbnail;
using TABP.Application.Commands.Images.Hotels.DeleteHotelImage;
using TABP.Application.Queries.Images.Hotels.GetHotelImageById;
using TABP.Application.Queries.Images.Hotels.GetHotelImages;
using TABP.Application.Queries.Images.Hotels.GetHotelThumbnail;
using TABP.Domain.Enums;

namespace TABP.Web.Controllers;

[ApiController]
[Route("api/hotels/{hotelId}/images")]
[Authorize(Roles = nameof(Role.Admin))]
public class HotelImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetImages(Guid hotelId)
    {
        var images = await _mediator.Send(new GetHotelImagesQuery
        {
            HotelId = hotelId
        });

        return Ok(images);
    }

    [HttpGet("{imageId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImage(Guid imageId)
    {
        var images = await _mediator.Send(new GetHotelImageByIdQuery
        {
            ImageId = imageId
        });

        return File(images.Image, $"image/{images.Extension}");
    }

    [HttpPost]
    public async Task<IActionResult> UploadImages(Guid hotelId,
        [FromForm(Name = "Images")] List<IFormFile> files)
    {
        await _mediator.Send(new CreateHotelImageCommand
        {
            HotelId = hotelId,
            Images = files,
        });

        return Created();
    }

    [HttpGet("thumbnail")]
    [AllowAnonymous]
    public async Task<IActionResult> GetThumbnail(Guid hotelId)
    {
        var images = await _mediator.Send(new GetHotelThumbnailQuery
        {
            HotelId = hotelId
        });

        return File(images.Image, $"image/{images.Extension}");
    }

    [HttpPost("thumbnail")]
    public async Task<IActionResult> UploadThumbnail(Guid hotelId,
        IFormFile image)
    {
        await _mediator.Send(new CreateHotelThumbnailCommand
        {
            HotelId = hotelId,
            Image = image
        });

        return Created();
    }

    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(Guid imageId, Guid hotelId)
    {
        await _mediator.Send(new DeleteHotelImageCommand
        {
            HotelId = hotelId,
            ImageId = imageId
        });

        return NoContent();
    }
}