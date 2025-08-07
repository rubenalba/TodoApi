using AutoMapper;
using Domain.DTOs;
using Domain.Models;

namespace Aplication.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskItem, TaskReadDto>();
        CreateMap<TaskCreateDto, TaskItem>();
        CreateMap<TaskUpdateDto, TaskItem>();
    }
}