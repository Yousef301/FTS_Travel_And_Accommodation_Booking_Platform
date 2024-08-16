using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Images.Rooms.CreateRoomImage;
using TABP.Application.Commands.Images.Rooms.DeleteRoomImage;
using TABP.Application.Queries.Images.Rooms.GetRoomImageById;
using TABP.Application.Queries.Images.Rooms.GetRoomImages;
using TABP.Domain.Enums;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/hotels/{hotelId:guid}/rooms/{roomId:guid}/images")]
[Authorize(Roles = nameof(Role.Admin))]
public class RoomImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves images for a specific room.
    /// </summary>
    /// <param name="roomId">The unique identifier of the room for which to retrieve images.</param>
    /// <returns>A list of images associated with the specified room.</returns>
    /// <response code="200">Returns the list of images for the room.</response>
    /// <response code="400">If the room ID is invalid or malformed.</response>
    /// <response code="500">If an internal server error occurs while retrieving the images.</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetImages(Guid roomId)
    {
        var images = await _mediator.Send(new GetRoomImagesQuery
        {
            RoomId = roomId
        });

        return Ok(images);
    }

    /// <summary>
    /// Retrieves a specific image by its unique identifier.
    /// </summary>
    /// <param name="imageId">The unique identifier of the image to retrieve.</param>
    /// <returns>The image file if found.</returns>
    /// <response code="200">Returns the requested image file.</response>
    /// <response code="404">If the image with the specified ID was not found.</response>
    /// <response code="500">If an internal server error occurs while retrieving the image.</response>
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

    /// <summary>
    /// Uploads images for a specific room in a hotel.
    /// </summary>
    /// <param name="roomId">The ID of the room for which images are being uploaded.</param>
    /// <param name="hotelId">The ID of the hotel to which the room belongs.</param>
    /// <param name="files">A list of images to be uploaded.</param>
    /// <response code="201">The images were successfully uploaded.</response>
    /// <response code="400">If the request is invalid or no files are provided.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to upload images for this room.</response>
    /// <response code="500">If an internal server error occurs while uploading the images.</response>
    [HttpPost]
    public async Task<IActionResult> UploadImages(Guid roomId,
        Guid hotelId,
        [FromForm(Name = "Images")] List<IFormFile> files)
    {
        await _mediator.Send(new CreateRoomImageCommand
        {
            HotelId = hotelId,
            RoomId = roomId,
            Images = files,
        });

        return Created();
    }

    /// <summary>
    /// Deletes a specific image associated with a room.
    /// </summary>
    /// <param name="imageId">The ID of the image to be deleted.</param>
    /// <param name="roomId">The ID of the room to which the image belongs.</param>
    /// <response code="204">The image was successfully deleted.</response>
    /// <response code="400">If the request is invalid or the image ID is not valid.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to delete the image for this room.</response>
    /// <response code="404">If the image is not found.</response>
    /// <response code="500">If an internal server error occurs while deleting the image.</response>
    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(Guid imageId,
        Guid roomId)
    {
        await _mediator.Send(new DeleteRoomImageCommand
        {
            RoomId = roomId,
            ImageId = imageId
        });

        return NoContent();
    }
}