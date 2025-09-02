using AutoMapper;
using Domain.DTOs;
using Domain.Models;

namespace Aplication.Mapping;

/// <summary>
///     AutoMapper profile for mapping between <see cref="TaskItem"/> and task-related DTOs:
///     <see cref="TaskReadDto"/>, <see cref="TaskCreateDto"/>, and <see cref="TaskUpdateDto"/>.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MappingProfile"/> class.
    ///     Configures mapping rules between domain models and DTOs.
    /// </summary>
    public MappingProfile()
    {
        // Map TaskItem to TaskReadDto (for reading tasks)
        CreateMap<TaskItem, TaskReadDto>();

        // Map TaskCreateDto to TaskItem (for creating tasks)
        CreateMap<TaskCreateDto, TaskItem>();

        // Map TaskUpdateDto to TaskItem (for updating tasks)
        CreateMap<TaskUpdateDto, TaskItem>();
    }
}