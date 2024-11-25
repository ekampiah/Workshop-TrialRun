using AutoMapper;
using Todo.Model;

namespace Todo.Core;

public class MappingProfiles : Profile{
    public MappingProfiles()
    {
        CreateMap<Item, Item>();
    }
}