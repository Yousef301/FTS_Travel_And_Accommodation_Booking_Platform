﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Images.Cities.CreateCityImage;
using TABP.Application.Commands.Images.Cities.CreateCityThumbnail;
using TABP.Application.Commands.Images.Cities.DeleteCityImage;
using TABP.Application.Queries.Images.Cities.GetCityImageById;
using TABP.Application.Queries.Images.Cities.GetCityImages;
using TABP.Application.Queries.Images.Cities.GetCityThumbnail;
using TABP.Domain.Enums;


namespace TABP.Web.Controllers;

[ApiController]
[Route("api/cities/{cityId}/images")]
[Authorize(Roles = nameof(Role.Admin))]
public class CityImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CityImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetImages(Guid cityId)
    {
        var images = await _mediator.Send(new GetCityImagesQuery
        {
            CityId = cityId
        });

        return Ok(images);
    }

    [HttpGet("{imageId:guid}")]
    [AllowAnonymous]
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
    [AllowAnonymous]
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
    public async Task<IActionResult> DeleteImage(Guid imageId, Guid cityId)
    {
        await _mediator.Send(new DeleteCityImageCommand
        {
            CityId = cityId,
            ImageId = imageId
        });

        return NoContent();
    }
}