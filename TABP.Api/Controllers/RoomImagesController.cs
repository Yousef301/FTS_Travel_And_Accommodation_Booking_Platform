using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Images.Rooms.CreateRoomImage;
using TABP.Application.Commands.Images.Rooms.DeleteRoomImage;
using TABP.Application.Queries.Images.Rooms.GetRoomImageById;
using TABP.Application.Queries.Images.Rooms.GetRoomImages;
using TABP.Domain.Enums;

namespace TABP.Web.Controllers;

[ApiController]
[Route("api/hotels/{hotelId:guid}/rooms/{roomId:guid}/images")]
[Authorize(Roles = nameof(Role.Admin))]
public class RoomImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetImages(Guid roomId, Guid hotelId)
    {
        var images = await _mediator.Send(new GetRoomImagesQuery
        {
            RoomId = roomId
        });

        return Ok(images);
    }

    [HttpGet("{imageId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImage(Guid imageId)
    {
        var images = await _mediator.Send(new GetRoomImageByIdQuery
        {
            ImageId = imageId
        });

        return File(images.Image, $"image/{images.Extension}");
    }

    [HttpPost]
    public async Task<IActionResult> UploadImages(Guid roomId,
        [FromForm(Name = "Images")] List<IFormFile> files)
    {
        await _mediator.Send(new CreateRoomImageCommand
        {
            RoomId = roomId,
            Images = files,
        });

        return Created();
    }

    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(Guid imageId, Guid roomId)
    {
        await _mediator.Send(new DeleteRoomImageCommand
        {
            RoomId = roomId,
            ImageId = imageId
        });

        return NoContent();
    }
}