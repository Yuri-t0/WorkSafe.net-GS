using WorkSafe.Api.Application.Common;
using WorkSafe.Api.Application.DTOs;
using WorkSafe.Api.Domain.Entities;

namespace WorkSafe.Api.Application.Services;

public class WorkstationAppService
{
    private readonly IWorkstationRepository _repository;

    public WorkstationAppService(IWorkstationRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkstationResponseDto?> GetByIdAsync(int id, Func<Workstation, List<LinkDto>> linkFactory, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        return ToResponseDto(entity, linkFactory(entity));
    }

    public async Task<PagedResult<WorkstationResponseDto>> SearchAsync(WorkstationSearchFilter filter, Func<Workstation, List<LinkDto>> linkFactory, CancellationToken cancellationToken = default)
    {
        var result = await _repository.SearchAsync(filter, cancellationToken);

        var dtoItems = result.Items
            .Select(e => ToResponseDto(e, linkFactory(e)))
            .ToArray();

        return new PagedResult<WorkstationResponseDto>
        {
            Items = dtoItems,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<WorkstationResponseDto> CreateAsync(WorkstationCreateDto dto, Func<Workstation, List<LinkDto>> linkFactory, CancellationToken cancellationToken = default)
    {
        var entity = new Workstation(
            dto.Name,
            dto.EmployeeName,
            dto.Department,
            dto.MonitorDistanceCm,
            dto.HasAdjustableChair,
            dto.HasFootrest);

        await _repository.AddAsync(entity, cancellationToken);

        return ToResponseDto(entity, linkFactory(entity));
    }

    public async Task<WorkstationResponseDto?> UpdateAsync(int id, WorkstationUpdateDto dto, Func<Workstation, List<LinkDto>> linkFactory, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        entity.Update(
            dto.Name,
            dto.EmployeeName,
            dto.Department,
            dto.MonitorDistanceCm,
            dto.HasAdjustableChair,
            dto.HasFootrest);

        await _repository.UpdateAsync(entity, cancellationToken);

        return ToResponseDto(entity, linkFactory(entity));
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        await _repository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static WorkstationResponseDto ToResponseDto(Workstation entity, List<LinkDto> links)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            EmployeeName = entity.EmployeeName,
            Department = entity.Department,
            MonitorDistanceCm = entity.MonitorDistanceCm,
            HasAdjustableChair = entity.HasAdjustableChair,
            HasFootrest = entity.HasFootrest,
            ErgonomicRiskLevel = entity.ErgonomicRiskLevel,
            LastEvaluationDate = entity.LastEvaluationDate,
            IsCompliant = entity.IsCompliant,
            Links = links
        };
}
