using Asp.Versioning;
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

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/hotels/{hotelId}/images")]
[Authorize(Roles = nameof(Role.Admin))]
public class HotelImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves images for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel for which to retrieve images.</param>
    /// <returns>A list of images associated with the specified hotel.</returns>
    /// <response code="200">Returns the list of images for the hotel.</response>
    /// <response code="400">If the hotel ID is invalid or malformed.</response>
    /// <response code="500">If an internal server error occurs while retrieving the images.</response>
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
        var images = await _mediator.Send(new GetHotelImageByIdQuery
        {
            ImageId = imageId
        });

        return File(images.Image, $"image/{images.Extension}");
    }

    /// <summary>
    /// Uploads images for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel to which the images will be uploaded.</param>
    /// <param name="files">The images to be uploaded. Supports multiple files.</param>
    /// <response code="201">If the images were successfully uploaded.</response>
    /// <response code="400">If the request is invalid or if the images are not provided.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to upload images for the hotel.</response>
    /// <response code="500">If an internal server error occurs while uploading the images.</response>
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

    /// <summary>
    /// Retrieves the thumbnail image for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel for which to retrieve the thumbnail.</param>
    /// <returns>The thumbnail image file if found.</returns>
    /// <response code="200">Returns the thumbnail image for the hotel.</response>
    /// <response code="404">If the thumbnail image for the specified hotel was not found.</response>
    /// <response code="500">If an internal server error occurs while retrieving the thumbnail.</response>
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

    /// <summary>
    /// Uploads a thumbnail image for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel for which the thumbnail is being uploaded.</param>
    /// <param name="image">The thumbnail image file to upload.</param>
    /// <response code="201">If the thumbnail image was successfully uploaded.</response>
    /// <response code="400">If the request is invalid or if no image file is provided.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to upload a thumbnail for the hotel.</response>
    /// <response code="500">If an internal server error occurs while uploading the thumbnail.</response>
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

    /// <summary>
    /// Deletes a specific image for a hotel.
    /// </summary>
    /// <param name="imageId">The unique identifier of the image to be deleted.</param>
    /// <param name="hotelId">The unique identifier of the hotel from which the image will be deleted.</param>
    /// <response code="204">If the image was successfully deleted.</response>
    /// <response code="400">If the request is invalid or if either the imageId or hotelId is missing.</response>
    /// <response code="404">If the image or hotel was not found.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to delete the image.</response>
    /// <response code="500">If an internal server error occurs while deleting the image.</response>
    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(Guid imageId,
        Guid hotelId)
    {
        await _mediator.Send(new DeleteHotelImageCommand
        {
            HotelId = hotelId,
            ImageId = imageId
        });

        return NoContent();
    }
}