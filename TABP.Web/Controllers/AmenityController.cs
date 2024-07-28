using Microsoft.AspNetCore.Mvc;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Web.Controllers;

[Route("api/amenity")]
public class AmenityController : ControllerBase
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityController(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAmenities()
    {
        var amenities = await _amenityRepository.GetAsync();

        return Ok(amenities);
    }
}