using System.ComponentModel.DataAnnotations;
using WorkSafe.Api.Domain.Enums;

namespace WorkSafe.Api.Application.DTOs;

public class WorkstationCreateDto
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string EmployeeName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Range(30, 100)]
    public int MonitorDistanceCm { get; set; }

    public bool HasAdjustableChair { get; set; }
    public bool HasFootrest { get; set; }
}

public class WorkstationUpdateDto : WorkstationCreateDto
{
    [Required]
    public int Id { get; set; }
}

public class WorkstationResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int MonitorDistanceCm { get; set; }
    public bool HasAdjustableChair { get; set; }
    public bool HasFootrest { get; set; }
    public ErgonomicRiskLevel ErgonomicRiskLevel { get; set; }
    public DateTime LastEvaluationDate { get; set; }
    public bool IsCompliant { get; set; }

    public List<LinkDto> Links { get; set; } = new();
}

public class LinkDto
{
    public string Rel { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
}

public class WorkstationSearchFilter
{
    public string? Department { get; set; }
    public ErgonomicRiskLevel? RiskLevel { get; set; }
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; } = "name";
    public string? SortDirection { get; set; } = "asc";
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 50 ? 50 : (value <= 0 ? 10 : value);
    }
}
