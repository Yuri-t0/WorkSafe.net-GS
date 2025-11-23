using Microsoft.EntityFrameworkCore;
using WorkSafe.Api.Application.Common;
using WorkSafe.Api.Application.DTOs;
using WorkSafe.Api.Application.Services;
using WorkSafe.Api.Domain.Entities;
using WorkSafe.Api.Infrastructure.Data;

namespace WorkSafe.Api.Infrastructure.Repositories;

public class WorkstationRepository : IWorkstationRepository
{
    private readonly AppDbContext _context;

    public WorkstationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Workstation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Workstations
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task AddAsync(Workstation workstation, CancellationToken cancellationToken = default)
    {
        await _context.Workstations.AddAsync(workstation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Workstation workstation, CancellationToken cancellationToken = default)
    {
        _context.Workstations.Update(workstation);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Workstation workstation, CancellationToken cancellationToken = default)
    {
        _context.Workstations.Remove(workstation);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<Workstation>> SearchAsync(WorkstationSearchFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Workstations.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Department))
        {
            query = query.Where(w => w.Department.Contains(filter.Department));
        }

        if (filter.RiskLevel.HasValue)
        {
            var level = filter.RiskLevel.Value;
            query = query.Where(w => w.ErgonomicRiskLevel == level);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.Trim();
            query = query.Where(w =>
                w.Name.Contains(term) ||
                w.EmployeeName.Contains(term));
        }

        // Sorting
        var sortBy = filter.SortBy?.ToLowerInvariant();
        var sortDirection = filter.SortDirection?.ToLowerInvariant() == "desc" ? "desc" : "asc";

        query = (sortBy, sortDirection) switch
        {
            ("department", "desc") => query.OrderByDescending(w => w.Department),
            ("department", _) => query.OrderBy(w => w.Department),

            ("risk", "desc") => query.OrderByDescending(w => w.ErgonomicRiskLevel),
            ("risk", _) => query.OrderBy(w => w.ErgonomicRiskLevel),

            ("name", "desc") => query.OrderByDescending(w => w.Name),
            ("name", _) => query.OrderBy(w => w.Name),

            _ => query.OrderBy(w => w.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var skip = (filter.PageNumber - 1) * filter.PageSize;

        var items = await query
            .Skip(skip)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Workstation>
        {
            Items = items,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }
}
