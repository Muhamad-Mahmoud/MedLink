using AutoMapper;
using MedLink.Application.DTOs.Doctors;
using MedLink.Domain.Entities.Medical;

namespace MedLink.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorSearchResultDto>()
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialization.Name))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
                .ForMember(dest => dest.AvailableToday, opt => opt.MapFrom(src => src.Availabilities.Any(a => a.Date.Date == DateTime.Today && !a.IsBooked)));
        }
    }
}
