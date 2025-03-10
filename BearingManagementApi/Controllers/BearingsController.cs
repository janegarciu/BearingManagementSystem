using BearingManagementApi.Models.Api.Requests;
using BearingManagementApi.Repositories.Abstractions;
using BearingManagementApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BearingManagementApi.Controllers;

[ApiController]
[Authorize]
[Route("bearings")]
public class BearingsController(IBearingsService bearingsService, IBearingsRepository bearingsRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await bearingsRepository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var bearing = await bearingsRepository.GetByIdAsync(id);
        return bearing != null ? Ok(bearing) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]BearingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Type))
            return BadRequest("Name and Type are required.");
        var result = await bearingsService.CreateAsync(request);
        if (result)
        {
            return Created();
        }
        return BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]BearingRequest bearing)
    {
        var result = await bearingsService.UpdateAsync(id, bearing);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute]int id)
        => await bearingsRepository.DeleteAsync(id) ? NoContent() : NotFound();
}