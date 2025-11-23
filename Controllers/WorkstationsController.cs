using Microsoft.AspNetCore.Mvc;
using WorkSafe.Api.Application.DTOs;
using WorkSafe.Api.Application.Services;
using WorkSafe.Api.Domain.Entities;

namespace WorkSafe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkstationsController : ControllerBase
{
    private readonly WorkstationAppService _service;

    public WorkstationsController(WorkstationAppService service)
    {
        _service = service;
    }

    // GET api/workstations/search
    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync(
        [FromQuery] string? department,
        [FromQuery] string? riskLevel,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDirection,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var filter = new WorkstationSearchFilter
        {
            Department = department,
            SearchTerm = searchTerm,
            SortBy = sortBy,
            SortDirection = sortDirection,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        if (!string.IsNullOrWhiteSpace(riskLevel) &&
            Enum.TryParse<Domain.Enums.ErgonomicRiskLevel>(riskLevel, true, out var parsedLevel))
        {
            filter.RiskLevel = parsedLevel;
        }

        var result = await _service.SearchAsync(filter, CreateLinks, cancellationToken);

        var collectionLinks = new List<LinkDto>
        {
            new()
            {
                Rel = "self",
                Href = GetCollectionUrl(filter),
                Method = "GET"
            },
            new()
            {
                Rel = "create",
                Href = Url.ActionLink(nameof(CreateAsync), "Workstations") ?? string.Empty,
                Method = "POST"
            }
        };

        return Ok(new
        {
            items = result.Items,
            pagination = new
            {
                result.PageNumber,
                result.PageSize,
                result.TotalCount,
                result.TotalPages
            },
            links = collectionLinks
        });
    }

    // GET api/workstations/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var workstation = await _service.GetByIdAsync(id, CreateLinks, cancellationToken);
        if (workstation is null)
            return NotFound(new ProblemDetails
            {
                Title = "Workstation not found",
                Detail = $"No workstation with id {id} was found.",
                Status = StatusCodes.Status404NotFound
            });

        return Ok(workstation);
    }

    // POST api/workstations
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] WorkstationCreateDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, CreateLinks, cancellationToken);

        var location = Url.ActionLink(nameof(GetByIdAsync), "Workstations", new { id = created.Id }) ?? string.Empty;

        return Created(location, created);
    }

    // PUT api/workstations/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] WorkstationUpdateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Id mismatch",
                Detail = "Route id and body id must be the same.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var updated = await _service.UpdateAsync(id, dto, CreateLinks, cancellationToken);

        if (updated is null)
            return NotFound(new ProblemDetails
            {
                Title = "Workstation not found",
                Detail = $"No workstation with id {id} was found.",
                Status = StatusCodes.Status404NotFound
            });

        return Ok(updated);
    }

    // DELETE api/workstations/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Workstation not found",
                Detail = $"No workstation with id {id} was found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return NoContent();
    }

    private List<LinkDto> CreateLinks(Workstation workstation)
    {
        var selfUrl = Url.ActionLink(nameof(GetByIdAsync), "Workstations", new { id = workstation.Id }) ?? string.Empty;
        var updateUrl = Url.ActionLink(nameof(UpdateAsync), "Workstations", new { id = workstation.Id }) ?? string.Empty;
        var deleteUrl = Url.ActionLink(nameof(DeleteAsync), "Workstations", new { id = workstation.Id }) ?? string.Empty;

        return new List<LinkDto>
        {
            new()
            {
                Rel = "self",
                Href = selfUrl,
                Method = "GET"
            },
            new()
            {
                Rel = "update",
                Href = updateUrl,
                Method = "PUT"
            },
            new()
            {
                Rel = "delete",
                Href = deleteUrl,
                Method = "DELETE"
            }
        };
    }

    private string GetCollectionUrl(WorkstationSearchFilter filter)
    {
        return Url.ActionLink(nameof(SearchAsync), "Workstations", new
        {
            department = filter.Department,
            riskLevel = filter.RiskLevel?.ToString(),
            searchTerm = filter.SearchTerm,
            sortBy = filter.SortBy,
            sortDirection = filter.SortDirection,
            pageNumber = filter.PageNumber,
            pageSize = filter.PageSize
        }) ?? string.Empty;
    }
}
