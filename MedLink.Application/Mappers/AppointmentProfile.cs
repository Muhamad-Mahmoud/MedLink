using AutoMapper;
using MedLink.Domain.Entities.Appointments;
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
            CreateMap<AppointmentDto, Appointment>().ReverseMap();
        }
    }
}
