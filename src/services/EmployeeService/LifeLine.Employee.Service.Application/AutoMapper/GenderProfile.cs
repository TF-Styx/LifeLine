using AutoMapper;
using LifeLine.Employee.Service.Domain.Models;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.AutoMapper
{
    public sealed class GenderProfile : Profile
    {
        public GenderProfile()
        {
            CreateMap<Gender, GenderResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
