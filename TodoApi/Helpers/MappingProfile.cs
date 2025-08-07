using AutoMapper;
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskItem, TaskReadDto>();
        CreateMap<TaskCreateDto, TaskItem>();
        CreateMap<TaskUpdateDto, TaskItem>();
    }
}