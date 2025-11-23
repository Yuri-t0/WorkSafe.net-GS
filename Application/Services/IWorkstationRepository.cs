using WorkSafe.Api.Application.Common;
using WorkSafe.Api.Application.DTOs;
using WorkSafe.Api.Domain.Entities;

namespace WorkSafe.Api.Application.Services;

public interface IWorkstationRepository
{
    Task<Workstation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Workstation workstation, CancellationToken cancellationToken = default);
    Task UpdateAsync(Workstation workstation, CancellationToken cancellationToken = default);
    Task DeleteAsync(Workstation workstation, CancellationToken cancellationToken = default);
    Task<PagedResult<Workstation>> SearchAsync(WorkstationSearchFilter filter, CancellationToken cancellationToken = default);
}
