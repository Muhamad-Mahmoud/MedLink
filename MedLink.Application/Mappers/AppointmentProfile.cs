using AutoMapper;
using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Entities.Payments;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Mappers
{
    public class AppointmentProfile :Profile
    {
        public AppointmentProfile()
        {
            // Appointment Mappings
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty))
                .ForMember(dest => dest.DoctorSpecialization,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Specialization : null))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Schedule.Date))
                .ForMember(dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.Schedule.StartTime))
                .ForMember(dest => dest.EndTime,
                    opt => opt.MapFrom(src => src.Schedule.EndTime))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentId,
                    opt => opt.MapFrom(src => src.Payment != null ? src.Payment.Id : (int?)null))
                .ForMember(dest => dest.PaymentStatus,
                    opt => opt.MapFrom(src => src.Payment != null ? src.Payment.Status.ToString() : null));

            // DoctorAvailability Mappings
            CreateMap<DoctorAvailability, DoctorAvailabilityDto>()
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty));

            // Payment Mappings
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Method,
                    opt => opt.MapFrom(src => src.Method.ToString()))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
