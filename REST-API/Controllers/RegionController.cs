using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.Region;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionController : ControllerBase
{
    private readonly IRegionRepository _regionRepository;

    public RegionController(IRegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }

    /// <summary>
    /// Get all regions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllRegions")]
    [ProducesResponseType(typeof(List<Region>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllRegions()
    {
        var regions = await _regionRepository.GetAllRegionsAsync();
        return Ok(regions);
    }

    /// <summary>
    /// Get region by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetRegionById/{id}")]
    [ProducesResponseType(typeof(Region), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRegionById(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var region = await _regionRepository.GetRegionByIdAsync(id);
        if (region == null)
        {
            return NotFound();
        }
        return Ok(region);
    }

    /// <summary>
    /// Add new region
    /// </summary>
    /// <param name="addRegionRequst"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddRegion")]
    [ProducesResponseType(typeof(Region), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddRegion(AddRegionRequest addRegionRequst)
    {
        if (addRegionRequst == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingRegion = await _regionRepository.GetRegionByIdAsync(addRegionRequst.RegionID);

        if (existingRegion != null)
        {
            return BadRequest("Region already exists");
        }

        var region = new Region
        {
            RegionID = addRegionRequst.RegionID,
            RegionDescription = addRegionRequst.RegionDescription
        };

        var addedRegion = await _regionRepository.AddRegionAsync(region);
        return CreatedAtAction(nameof(GetRegionById), new { id = addedRegion.RegionID }, addedRegion);
    }

    /// <summary>
    /// Edit region
    /// </summary>
    /// <param name="editRegionRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditRegion")]
    [ProducesResponseType(typeof(Region), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditRegion(EditRegionRequest editRegionRequest)
    {
        if (editRegionRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var region = new Region
        {
            RegionID = editRegionRequest.RegionID,
            RegionDescription = editRegionRequest.RegionDescription
        };

        var editedRegion = await _regionRepository.EditRegionAsync(region);

        if (editedRegion == null)
        {
            return NotFound();
        }

        return Ok(editedRegion);
    }

    /// <summary>
    /// Delete region
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteRegion/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRegion(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var result = await _regionRepository.DeleteRegionAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return Ok($"Region with id: {id} is successfully deleted!");
    }
}