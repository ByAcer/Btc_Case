using AutoMapper;
using Instruction.Domain.Models;
using Instruction.Domain.ValueObjects.DTOs.Requests;

namespace Instruction.ApplicationService.MapProfiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<InstructionOrder, InstructionOrderCreateRequestDto>().ReverseMap();
        }
    }
}
